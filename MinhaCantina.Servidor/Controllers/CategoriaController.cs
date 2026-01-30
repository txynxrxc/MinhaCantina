using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaCantina.Biblioteca.DTOs;
using MinhaCantina.Biblioteca.Modelos;
using MinhaCantina.Servidor.Dados;
using MySqlConnector;

namespace MinhaCantina.Servidor.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriaController(MinhaCantinaContexto contextoCantina) : ControllerBase
{
	private MinhaCantinaContexto _contexto = contextoCantina;
	// Criar uma propriedade privada do tipo MinhaCantinaContexto
	// O nome da propriedade será _contexto;

	// [Atributo]
	[HttpPost("criar")]
	public IActionResult CriarCategoria([FromBody] CategoriaRegistroDto requisicao)
	{
		Categoria categoria;
		try
		{
			categoria = Categoria.Criar(requisicao.Nome.ToUpper());
			_contexto.Categorias.Add(categoria);
			_contexto.SaveChanges();
		}
		catch (DbUpdateException excecao)
		{
			var excecaoInterna = excecao.InnerException;

			if (excecaoInterna is MySqlException excecaoMySql)
			{
				if (excecaoMySql.Number == 1062)
				{
					return StatusCode(400, "Essa categoria já existe");
				}
			}

			throw excecao;
		}
		catch (Exception excecao)
		{
			return StatusCode(500, $"Ocorreu um erro inesperado: {excecao.Message}");
		}

		return StatusCode(201, categoria);
	}

	// [Atributo]
	[HttpGet("pegar/{id}")] // -> Verbo HTTP
	public IActionResult PegarCategoria(int id)
	{
		var categoria = _contexto.Categorias.Find(id);

		if (categoria is null)
		{
			return StatusCode(404, "Categoria não encontrada");
		}

		return StatusCode(200, categoria);
	}

	[HttpGet("pegar_todos")]
	public IActionResult PegarTodasCategorias()
	{
		var categorias = _contexto.Categorias.ToList();
		return StatusCode(200, categorias);
	}
}