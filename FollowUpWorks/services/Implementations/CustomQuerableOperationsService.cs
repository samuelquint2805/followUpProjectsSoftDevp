using AutoMapper;
using FollowUpWorks.Core;
using FollowUpWorks.Models;
using FollowUpWorks.services.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace FollowUpWorks.services.Implementations
{
    public class CustomQuerableOperationsService
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private const string CACHE_KEY_PREFIX = "EntityList_";

        // Almacena listas separadas por tipo de entidad
        private readonly Dictionary<Type, IList<object>> _data = new();

        public CustomQuerableOperationsService(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }
        private string GetCacheKey<TEntity>()
        {
            return $"{CACHE_KEY_PREFIX}{typeof(TEntity).Name}";
        }
        private List<TEntity> GetEntityList<TEntity>() where TEntity : class, iID
        {
            string cacheKey = GetCacheKey<TEntity>();

            if (!_cache.TryGetValue(cacheKey, out List<TEntity> list))
            {
                list = new List<TEntity>();
                _cache.Set(cacheKey, list, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal,
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });
            }

            return list;
        }

        // ======================================
        // CREATE
        // ======================================
        public Response<TDTO> CreateGeneric<TEntity, TDTO>(TDTO dto)
             where TEntity : class, iID
        {
            try
            {
                TEntity entity = _mapper.Map<TEntity>(dto);
                entity.Id = Guid.NewGuid();

                var list = GetEntityList<TEntity>();
                list.Add(entity);

                // Actualizar cache
                string cacheKey = GetCacheKey<TEntity>();
                _cache.Set(cacheKey, list);

                TDTO resultDto = _mapper.Map<TDTO>(entity);

                return new Response<TDTO>(
                    resultDto,
                    "Registro creado correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<TDTO>(
                    "Error al crear el registro",
                    new List<string> { ex.Message }
                );
            }
        }
        // ======================================
        // UPDATE
        // ======================================
        public Response<TDTO> UpdateGeneric<TEntity, TDTO>(Guid id, TDTO dto)
            where TEntity : class, iID
        {
            try
            {
                var list = GetEntityList<TEntity>();
                var entity = list.FirstOrDefault(e => e.Id == id);

                if (entity == null)
                {
                    return new Response<TDTO>(
                        "Error al actualizar el registro",
                        new List<string> { "Registro no encontrado" }
                    );
                }

                // Mapear los cambios del DTO a la entidad existente
                _mapper.Map(dto, entity);
                entity.Id = id; // Asegurar que el Id no cambie

                // Actualizar cache
                string cacheKey = GetCacheKey<TEntity>();
                _cache.Set(cacheKey, list);

                TDTO resultDto = _mapper.Map<TDTO>(entity);

                return new Response<TDTO>(
                    resultDto,
                    "Registro actualizado correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<TDTO>(
                    "Error al actualizar el registro",
                    new List<string> { ex.Message }
                );
            }
        }

        // ======================================
        // DELETE
        // ======================================
        public Response<object> DeleteGeneric<TEntity>(Guid id)
            where TEntity : class, iID
        {
            try
            {
                var list = GetEntityList<TEntity>();
                var entity = list.FirstOrDefault(e => e.Id == id);

                if (entity == null)
                {
                    return new Response<object>(
                        "Error al eliminar el registro",
                        new List<string> { "Registro no encontrado" }
                    );
                }

                list.Remove(entity);

                // Actualizar cache
                string cacheKey = GetCacheKey<TEntity>();
                _cache.Set(cacheKey, list);

                return new Response<object>(
                    null,
                    "Registro eliminado correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<object>(
                    "Error al eliminar el registro",
                    new List<string> { ex.Message }
                );
            }
        }


        // ======================================
        // GET ONE
        // ======================================
        public Response<TDTO> GetOneGeneric<TEntity, TDTO>(Guid id)
              where TEntity : class, iID
        {
            try
            {
                var list = GetEntityList<TEntity>();
                var entity = list.FirstOrDefault(e => e.Id == id);

                if (entity == null)
                {
                    return new Response<TDTO>(
                        "Error al obtener el registro",
                        new List<string> { "Registro no encontrado" }
                    );
                }

                TDTO dto = _mapper.Map<TDTO>(entity);

                return new Response<TDTO>(
                    dto,
                    "Registro obtenido correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<TDTO>(
                    "Error al obtener el registro",
                    new List<string> { ex.Message }
                );
            }
        }


        // ======================================
        // GET ALL
        // ======================================
        public Response<List<TDTO>> GetAllGeneric<TEntity, TDTO>()
            where TEntity : class, iID
        {
            try
            {
                var list = GetEntityList<TEntity>();

                if (!list.Any())
                {
                    return new Response<List<TDTO>>(
                        new List<TDTO>(),
                        "No hay registros disponibles"
                    );
                }

                List<TDTO> resultDtoList = _mapper.Map<List<TDTO>>(list);

                return new Response<List<TDTO>>(
                    resultDtoList,
                    "Lista de registros obtenida correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<List<TDTO>>(
                    "Error al obtener la lista de registros",
                    new List<string> { ex.Message }
                );
            }
        }
        public Response<List<TDTO>> GetListFromModel<TEntity, TDTO>(IEnumerable<TEntity> modelList)
           where TEntity : class, iID
        {
            try
            {
                if (modelList == null || !modelList.Any())
                {
                    return new Response<List<TDTO>>(
                        "Lista de modelos vacía o nula",
                        new List<string> { "La colección de entrada no contiene registros" }
                    );
                }

                List<TDTO> resultDtoList = _mapper.Map<List<TDTO>>(modelList);

                return new Response<List<TDTO>>(
                    resultDtoList,
                    "Lista de registros obtenida y mapeada correctamente"
                );
            }
            catch (Exception ex)
            {
                return new Response<List<TDTO>>(
                    "Error al procesar la lista de modelos",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
