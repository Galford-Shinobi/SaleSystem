﻿namespace SaleSystem.Common.Response
{
    public class GenericResponse<TObject>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public TObject DirectObject { get; set; }
        public List<TObject> ListObjet { get; set; }
        public IQueryable<TObject> MyIQueryable { get; set; }
        public ICollection<TObject> MyCollection { get; set; }
    }
}
