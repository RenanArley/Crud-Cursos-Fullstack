using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_cursos.Repository
{
    public class CursoAtivoException : Exception
    {
        public CursoAtivoException()
            : base("Não é possivel excluir um curso ativo.") 
        { 
        }
    }
}