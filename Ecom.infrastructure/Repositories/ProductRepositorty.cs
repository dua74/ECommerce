using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepositorty : GenericRepository<Product>,IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepositorty(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
          if(productDTO == null) return false;
          var product = mapper.Map<Product>(productDTO);
           await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName= path,
                ProductId = product.Id,

            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;


        }

        
        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO == null)
            {
                return false; ;
            }
            var findProduct = await context.Products.Include(m=>m.Category).Include(m => m.Photos).FirstOrDefaultAsync(x => x.Id == updateProductDTO.Id);
            if (findProduct == null)
            {
                return false;
            }
            mapper.Map(updateProductDTO, findProduct);
            var findPhoto = await context.Photos.Where(x => x.ProductId == updateProductDTO.Id).ToListAsync();
         
                foreach (var item in findPhoto)
                {
                     imageManagementService.DeleteImageAsync(item.ImageName);
                }
                context.Photos.RemoveRange(findPhoto);
            var ImagePath = await imageManagementService.AddImageAsync(updateProductDTO.Photo, updateProductDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDTO.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }



        public async Task DeleteAsync(Product product)
        {
           var photo= await context.Photos.Where(x => x.ProductId == product.Id).ToListAsync();
            foreach (var item in photo)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            //context.Photos.RemoveRange(photo);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

    }
}
