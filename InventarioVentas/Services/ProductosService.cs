using AutoMapper;
using Dominio.Productos;
using InventarioVentas.DTOs.ProductoDtos;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Services
{
    public class ProductosService:ICommonService<ProductDto,ProductInsertDto,ProductUpdateDto>
    {
       //private IRepository<Producto> _productopository;
        private IMapper _mapper;
        private EntitiesDomainProductos _entities;
        
        public List<string> Errors { get; }
        public ProductosService( IMapper mapper,EntitiesDomainProductos entitiDomainProductos)
        {
           // _productopository = productopository;
            _mapper = mapper;
            Errors = new List<string>();
            _entities = entitiDomainProductos;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
           // var producto = await _productopository.GetAll(); servicio de repositorio generico ICollection<Producto>
            var producto = await _entities.ProductosRepository.GetAll(); //servicio de repositorio especifico
            return producto.Select(b => new ProductDto
            {
                IdProducto = b.IdProducto,
                Nombre = b.Nombre,
                Descripcion = b.Descripcion,
                Precio = b.Precio,
                Stock = b.Stock,
                FechaCreacion = b.FechaCreacion,    

            });
        }

        public async Task<ProductDto> GetId(int id)
        {
            var producto = await _entities.ProductosRepository.GetById(id); 
            if (producto != null)
            {
                var responce = new ProductDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    FechaCreacion = producto.FechaCreacion,
                };
                return responce;
            }
            return null;
        }

        public async Task<ProductDto> Add(ProductInsertDto productInsertDto)
        {
            var producto= _mapper.Map<Producto>(productInsertDto); //necesito un beer a partir de (beerInsertDto)
            await _entities.ProductosRepository.Add(producto);
             _entities.GuardarTransacciones();
            //necestio un beerDto a partir de (beer)
            /* var beerDto = new BeerDto
             {
                 Id = beer.BeerId,
                 Name = beer.Name,
                 Alcohol = beer.Alcohol,
                 BrandId = beer.BrandId
             };*/
            var beerDto = _mapper.Map<ProductDto>(producto);
            
            return beerDto;
        }
        public async Task<ProductDto> Update(int id, ProductUpdateDto productUpdateDto)
        {
            var producto = await _entities.ProductosRepository.GetById(id);
            if (producto != null)
            {
                //apartir de beerUpdateDto necesito un beer
                producto = _mapper.Map<ProductUpdateDto, Producto>(productUpdateDto, producto);
                //beer.Name = beerUpdateDto.Name;
                //beer.Alcohol = beerUpdateDto.Alcohol;
                // beer.BrandId = beerUpdateDto.BrandId;
                _entities.ProductosRepository.Actualizar(producto);
                _entities.GuardarTransacciones();
                var productDto = new ProductDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    FechaCreacion = producto.FechaCreacion,
                };
                return productDto;

            }
            return null;
        }
        public async Task<ProductDto> Delete(int id)
        {
            var producto = await _entities.ProductosRepository.GetById(id);
            if (producto != null)
            {
                var productDto = new ProductDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock= producto.Stock,
                    FechaCreacion = producto.FechaCreacion,
                };
                _entities.ProductosRepository.Delete(producto);
                _entities.GuardarTransacciones();

                return productDto;

            }
            return null;

        }

        
        public bool Validate(ProductInsertDto dto)
        {
            if (_entities.ProductosRepository.Search(b => b.Nombre == dto.Nombre).Count() > 0)
           {
                Errors.Add("Ya existe un Producto con ese nombre");
                return false;
           }
               return true;
        }

        public bool Validate(ProductUpdateDto dto)
        {
            if (_entities.ProductosRepository.Search(b => b.Nombre == dto.Nombre
                && dto.Id != b.IdProducto).Count() > 0)
               
            {
                Errors.Add("Ya existe un Producto con ese nombre");
                return false;
            }
            return true;
        }
    }
}
