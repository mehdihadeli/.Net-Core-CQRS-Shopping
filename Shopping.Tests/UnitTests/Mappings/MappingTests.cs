using AutoMapper;
using Shopping.Core.Domains;
using Shopping.Core.Dtos;
using Shopping.Core.Dtos.User;
using Shopping.Core.ViewModels;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.Mappings
{
    [Collection("MappingCollection")]
    public class MappingTests
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;

        public MappingTests(MappingFixture mappingFixture)
        {
            _mapper = mappingFixture.Mapper;
            _configurationProvider = mappingFixture.ConfigurationProvider;
        }
        
         [Fact]
        public void Should_Have_Valid_Configuration()
        {
            _configurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_Category_To_CategoryDto()
        {
            var entity = new Category();

            var result = _mapper.Map<CategoryDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<CategoryDto>();
        }
        [Fact]
        public void Should_Map_ApplicationUser_To_UserDetailInfoDto()
        {
            var entity = new ApplicationUser();

            var result = _mapper.Map<UserDetailInfoDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserDetailInfoDto>();
        }

        [Fact]
        public void Should_Map_Customer_To_CustomerLookupDto()
        {
            var entity = new Customer();

            var result = _mapper.Map<CustomerLookupDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<CustomerLookupDto>();
        }

        [Fact]
        public void Should_Map_Product_To_ProductDetailVm()
        {
            var entity = new Product();

            var result = _mapper.Map<ProductDetailVm>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ProductDetailVm>();
        }

        [Fact]
        public void Should_Map_Product_To_ProductDto()
        {
            var entity = new Product();

            var result = _mapper.Map<ProductDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ProductDto>();
        }

        [Fact]
        public void Should_Map_Product_To_ProductRecordDto()
        {
            var entity = new Product();

            var result = _mapper.Map<ProductRecordDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ProductRecordDto>();
        }

        [Fact]
        public void Should_Map_Customer_To_CustomerDetailVm()
        {
            var entity = new Customer();

            var result = _mapper.Map<CustomerDetailVm>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<CustomerDetailVm>();
        }

        [Fact]
        public void Should_Map_Employee_To_EmployeeLookupDto()
        {
            var entity = new Employee();

            var result = _mapper.Map<EmployeeLookupDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<EmployeeLookupDto>();
        }

        [Fact]
        public void Should_Map_EmployeeTerritory_To_EmployeeTerritoryDto()
        {
            var entity = new EmployeeTerritory();

            var result = _mapper.Map<EmployeeTerritoryDto>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<EmployeeTerritoryDto>();
        }
    }
}