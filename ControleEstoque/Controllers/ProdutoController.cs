using Microsoft.AspNetCore.Mvc;
using ControleEstoque.Models;
using ControleEstoque.DTOs;
using ControleEstoque.Services;

namespace ControleEstoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        // GET api/produto
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _service.ObterTodos();


            var resultado = produtos.Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Quantidade = p.Quantidade,
                Preco = p.Preco,
                EstoqueBaixo = p.Quantidade < 5
            });

            return Ok(resultado);
        }

        // POST api/produto
        [HttpPost]
        public IActionResult CriarProduto(CriarProdutoDto dto)
        {

            var produto = new Produto
            {
                Nome = dto.Nome,
                Quantidade = dto.Quantidade,
                Preco = dto.Preco
            };

            var criado = _service.Criar(produto);

            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}/entrada")]
        public IActionResult EntradaEstoque(int id, [FromBody] QuantidadeDto dto)
        {
            if (dto.Quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }
            
            var produtos = _service.ObterTodos();

            foreach (var produto in produtos)
            {
                if (produto.Id == id)
                {
                    produto.Quantidade += dto.Quantidade;
                    return Ok(produto);
                }
            }

            return NotFound("Produto não encontrado.");
        }

        [HttpPut("{id}/saida")]
        public IActionResult SaidaEstoque(int id, [FromBody] QuantidadeDto dto)
        {
            if (dto.Quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }

            var produtos = _service.ObterTodos();

            foreach (var produto in produtos)
            {
                if (produto.Id == id)
                {
                    if (produto.Quantidade < dto.Quantidade)
                    {
                        return BadRequest("Estoque insuficiente.");
                    }

                    produto.Quantidade -= dto.Quantidade;
                    return Ok(produto);
                }
            }

            return NotFound("Produto não encontrado.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var produtos = _service.ObterTodos();

            var produto = produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound("Produto não encontrado");

            produtos.Remove(produto);

            return NoContent(); // 204
        }



    }
}
