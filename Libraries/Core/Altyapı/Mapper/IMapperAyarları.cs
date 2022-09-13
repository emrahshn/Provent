using AutoMapper;
using System;

namespace Core.Altyapı.Mapper
{
    public interface IMapperAyarları
    {
        Action<IMapperConfigurationExpression> GetConfiguration();
        int Order { get; }
    }
}
