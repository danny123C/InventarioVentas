namespace InventarioVentas.Services
{
    public interface ICommonService<T,TI,TU>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetId(int id);
        Task<T> Add(TI productInsertDto);
        Task<T> Update(int id, TU productUpdateDto);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
        public List<string> Errors { get; }
    }
}
