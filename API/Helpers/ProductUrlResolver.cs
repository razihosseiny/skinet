using AutoMapper;
using Core.Entities;
using API.Dtos;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(destination.PictureUrl))
            {
                return _config["ApiUrl"] + destination.PictureUrl;
            }
            return _config["ApiUrl"];
        }
    }
}