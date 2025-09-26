using AutoMapper;
using Microsoft.Extensions.Logging;
using SportGo.Repository;
using SportGo.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<SportGoDbContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;

        public BaseService(IUnitOfWork<SportGoDbContext> unitOfWork, ILogger<T> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


    }
}
