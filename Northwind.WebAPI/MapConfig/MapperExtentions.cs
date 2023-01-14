using MapsterMapper;
using Northwind.Utility.Model.ViewModel;

namespace Northwind.WebAPI.MapConfig
{
    public static class MapperExtentions
    {
        /// <summary>
        /// Mappster 多合一擴充
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static TDestination MulitMap<TDestination>(this IMapper mapper, params object[] sources) where TDestination : new()
        {

            TDestination destination = new();
            if (!sources.Any())
                return destination;


            foreach (var src in sources)
            {

                destination = (TDestination?)mapper.Map(src, destination, src.GetType(), destination.GetType());

            }


            return destination;
        }
    }
}
