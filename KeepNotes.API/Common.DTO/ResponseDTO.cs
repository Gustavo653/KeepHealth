﻿using Common.Functions;
using System.Diagnostics;
using System.Reflection;

namespace Common.DTO
{
    public class ResponseDTO
    {
        public int Code { get; set; } = 200;
        public string Message { get; set; } = "Operação concluída!";
        public DateTime Date { get; set; } = DateTime.Now;
        public object Object { get; set; } = new object();
        public object Error { get; set; } = new object();
        public Stopwatch Elapsed { get; set; } = Stopwatch.StartNew();
        public void SetError(Exception ex)
        {
            Code = 500;
            Message = "Ocorreu um erro!";
            Error = new { message = ex.Message, innerMessage = ex.InnerException?.Message, stackTrace = ex.StackTrace };
        }
        public void SetNotImplemented(string? cod = null)
        {
            Code = 500;
            Message = $"Método não implementado! {cod}";
        }
    }
}