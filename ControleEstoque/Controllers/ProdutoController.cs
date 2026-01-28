using Microsoft.AspNetCore.Mvc;
using ControleEstoque.Models;
using ControleEstoque.DTOs;
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
            return CreatedAtAction(nameof(Get), new { id = criado.Id }, criado);
        }

        // PUT api/produto/{id}
        [HttpPut("{id}")]
        public IActionResult AtualizarProduto(int id, CriarProdutoDto dto)
        {
            var produto = _service.Atualizar(id, dto);

            if (produto == null)
                return NotFound("Produto não encontrado");

            return Ok(produto);
        }

        // PUT api/produto/{id}/entrada
        [HttpPut("{id}/entrada")]
        public IActionResult EntradaEstoque(int id, QuantidadeDto dto)
        {
            var produto = _service.EntradaEstoque(id, dto.Quantidade);

            if (produto == null)
                return BadRequest(new { mensagem = "Produto não encontrado ou quantidade inválida" });

            return Ok(produto);
        }

        // PUT api/produto/{id}/saida
        [HttpPut("{id}/saida")]
        public IActionResult SaidaEstoque(int id, QuantidadeDto dto)
        {
            var produto = _service.SaidaEstoque(id, dto.Quantidade);

            if (produto == null)
                return BadRequest(new { mensagem = "Produto não encontrado ou estoque insuficiente" });

            return Ok(produto);
        }

        // DELETE api/produto/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var removido = _service.Remover(id);

            if (!removido)
                return NotFound(new { mensagem = "Produto não encontrado" });

            return NoContent();
        }
    }
}
