using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService ;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;
        public ICategoryRepository CategoryRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public IProductRepository ProductRepository { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;



            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepositorty(_context, _mapper, _imageManagementService);
            PhotoRepository = new PhototRepository(_context);
            Auth = new AuthRepository(userManager, emailService, signInManager, token);
            
        }
    } 
}
  