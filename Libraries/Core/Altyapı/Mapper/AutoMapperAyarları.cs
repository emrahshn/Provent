using System;
using System.Collections.Generic;
using AutoMapper;

namespace Core.Altyapı.Mapper
{
    public static class AutoMapperAyarları
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        public static void Init(List<Action<IMapperConfigurationExpression>> configurationActions)
        {
            if (configurationActions == null)
                throw new ArgumentNullException("configurationActions");

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var ca in configurationActions)
                    ca(cfg);
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}
