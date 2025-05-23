using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
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

        public async Task<ReturnProductDTO> GetAllAsync(ProductParams productParams)
        {
            var query =   context.Products.Include(m => m.Category).Include(m => m.Photos).AsNoTracking();

            //filter by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWord = productParams.Search.Split(' '); 
                query = query.Where(x => searchWord.All(word => x.Name.ToLower().Contains(word.ToLower()) 
                ||
                x.Description.ToLower().Contains(word.ToLower())));

            }

            //filter by category id
            if (productParams.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == productParams.CategoryId);

            query = string.IsNullOrEmpty( productParams.sort) ? query.OrderBy(x => x.Name) : productParams.sort switch
            {
                "PriceAsc" => query.OrderBy(x => x.NewPrice),
                "PriceDesc" => query.OrderByDescending(x => x.NewPrice),
                _ => query.OrderBy(x => x.Name),
            };

            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount =query.Count();
            query = query.Skip((productParams.pageNumber - 1) * productParams.pageSize).Take(productParams.pageSize);


            returnProductDTO.products =  mapper.Map<List<ProductDTO>>(query);
            return returnProductDTO;


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
