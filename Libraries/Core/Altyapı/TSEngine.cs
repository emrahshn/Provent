using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Core.Yapılandırma;
using Core.Altyapı.BağımlılıkYönetimi;
using Core.Altyapı.Mapper;
using Microsoft.AspNet.SignalR;

namespace Core.Altyapı
{
    public class TSEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Utilities

        protected virtual void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sırala
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        protected virtual void RegisterDependencies(TSConfig config)
        {
            var builder = new ContainerBuilder();

            //bağımlılıklar
            var typeFinder = new WebAppTypeFinder();
            builder.RegisterInstance(config).As<TSConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //Diğer assmblyler tarafından sağlanan bağımlılıkları kaydet
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sırala
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder, config);

            var container = builder.Build();
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
            this._containerManager = new ContainerManager(container);

            //bağımlılık çözücü ayarla
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        protected virtual void RegisterMapperConfiguration(TSConfig config)
        {
            //bağımlılıklar
            var typeFinder = new WebAppTypeFinder();

            //Diğer assmblyler tarafından sağlanan bağımlılıkları kaydet
            var mcTypes = typeFinder.FindClassesOfType<IMapperAyarları>();
            var mcInstances = new List<IMapperAyarları>();
            foreach (var mcType in mcTypes)
                mcInstances.Add((IMapperAyarları)Activator.CreateInstance(mcType));
            //sırala
            mcInstances = mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //yapılandırmayı al
            var configurationActions = new List<Action<IMapperConfigurationExpression>>();
            foreach (var mc in mcInstances)
                configurationActions.Add(mc.GetConfiguration());
            //kaydet
            AutoMapperAyarları.Init(configurationActions);
        }

        #endregion

        #region Methods
        public void Initialize(TSConfig config)
        {
            //bağımlılıkları kaydet
            RegisterDependencies(config);

            //mapper ayarlarını kaydet
            RegisterMapperConfiguration(config);

            //görevleri başlat
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }

        }
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }
        public T ResolveUnregistered<T>() where T : class
        {
            return ContainerManager.ResolveUnregistered<T>();
        }
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties
        public virtual ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}
