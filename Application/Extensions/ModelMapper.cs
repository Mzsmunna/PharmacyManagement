using Application.Dtos;
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

        public static List<TModel> ToModelList<TModel, TEntity>(this List<TEntity> entities) where TEntity : BaseEntity where TModel : class
        {
            var data = new List<TModel>();
            foreach (var entity in entities)
            {
                var row = entity.ToModel<TModel, TEntity>();
                data.Add(row);
            }
            return data;
        }

        public static IEnumerable<TModel> ToModelList<TModel, TEntity>(this IEnumerable<TEntity> entities) where TEntity : BaseEntity where TModel : class
        {
            var data = new List<TModel>();
            foreach (var entity in entities.ToList())
            {
                var row = entity.ToModel<TModel, TEntity>();
                data.Add(row);
            }
            return data;
        }

        public static TEntity ToEntity<TEntity, TModel>(this TModel model) where TEntity : BaseEntity where TModel : class
        {
            return JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(model), jsonSrlizeConfig)!;
        }

        public static List<TEntity> ToEntityList<TEntity, TModel>(this List<TModel> models) where TEntity : BaseEntity where TModel : class
        {
            var data = new List<TEntity>();
            foreach (var model in models)
            {
                var row = model.ToEntity<TEntity, TModel>();
                data.Add(row);
            }
            return data;
        }

        public static IEnumerable<TEntity> ToEntityList<TEntity, TModel>(this IEnumerable<TModel> models) where TEntity : BaseEntity where TModel : class
        {
            var data = new List<TEntity>();
            foreach (var model in models)
            {
                var row = model.ToEntity<TEntity, TModel>();
                data.Add(row);
            }
            return data;
        }
    }
}
