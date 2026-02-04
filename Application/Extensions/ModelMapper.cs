using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class ModelMapper
    {
        public static readonly JsonSerializerSettings jsonSrlizeConfig = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
        
        public static TModel ToModel<TModel, TEntity>(this TEntity entity) where TEntity : BaseEntity where TModel : class
        {
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(entity), jsonSrlizeConfig)!;
        }

        public static TEntity ToEntity<TEntity, TModel>(this TModel model) where TEntity : BaseEntity where TModel : class
        {
            return JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(model), jsonSrlizeConfig)!;
        }
    }
}
