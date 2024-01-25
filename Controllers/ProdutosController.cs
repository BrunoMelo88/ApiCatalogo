using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/<ProdutosController>
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.AsNoTracking().Take(10).ToList();
                if (produtos is null)
                {
                    return NotFound($"Produtos não encontrados...");
                }
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }

        }

        [HttpGet("primeiro")]
        public ActionResult<Produto> GetPrimeiro()
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault();
                if (produto is null)
                {
                    return NotFound($"Produto não encontrado...");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }

        }

        // GET api/<ProdutosController>/5
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto não encontrado...");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }

        }

        // POST api/<ProdutosController>
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest();

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }

        }

        // PUT api/<ProdutosController>/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                //return NoContent();
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }


        }
        // DELETE api/<ProdutosController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
                //var produto = _context.Produtos.Find(id);

                if (produto is null)
                {
                    return NotFound($"Produto não localizado...");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atratar sua solicitação.");
            }

        }
    }
}
