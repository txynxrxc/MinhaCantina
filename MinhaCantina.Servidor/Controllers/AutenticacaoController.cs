using Microsoft.AspNetCore.Mvc;
using MinhaCantina.Biblioteca.DTOs;
using MinhaCantina.Biblioteca.Modelos;
using MinhaCantina.Servidor.Dados;

namespace MinhaCantina.Servidor.Controllers;

[Route("[controller]")]
[ApiController]
public class AutenticacaoController(MinhaCantinaContexto contextoCantina) : ControllerBase
{
	// modificadorAcesso tipoCampo NomeCampo = Valor;
	private readonly MinhaCantinaContexto _contexto = contextoCantina;

	// GET, POST, PATCH/PUT, DELETE
	// Pegar, Criar, Alterar, Excluir
	// Sintaxe do Método
	// modificadorAcesso tipoRetorno NomeMetodo(tipoParametro nomeParametro)
	[HttpGet("/login")]
	public IActionResult Login([FromBody] UsuarioLoginDto requisicao)
	{
		// 1º: Verificar se o usuário existe
		Usuario? usuarioDoBanco = _contexto.Usuarios.FirstOrDefault(usuario => usuario.Username == requisicao.Username);

		// 2º: Se não existir, retorna um erro de login
		if (usuarioDoBanco is null)
		{
			return StatusCode(400, "Usuário e/ou senha estão incorretos");
		}

		// 3º: Se existir, verificar as senhas se são iguais
		bool senhasIguais = requisicao.Senha == usuarioDoBanco.Senha;

		// 4º: Se não for igual, retorna um erro de login
		if(senhasIguais == false)
		{
			return StatusCode(400, "Usuário e/ou senha estão incorretos");
		}
		// 5º: Se existir, retorna status 200
		return StatusCode(200, new UsuarioRespostaDto() { 
			Id = usuarioDoBanco.Id,
			Nome = usuarioDoBanco.Nome,
			Username = usuarioDoBanco.Username
		});

	}

	[HttpPost("/cadastrar")]
	public IActionResult Cadastrar([FromBody] UsuarioRegistroDto requisicao)
	{
		Usuario? usuarioDoBanco = _contexto.Usuarios.FirstOrDefault(usuario => usuario.Username == requisicao.Username);

		if (usuarioDoBanco is not null)
		{
			return StatusCode(400, "Usuário já existe");
		}

		UsuarioRespostaDto respostaDto = new();

		try
		{
			Usuario novoUsuario = Usuario.Criar(requisicao.Nome, requisicao.Senha, requisicao.Username);

			_contexto.Usuarios.Add(novoUsuario);
			_contexto.SaveChanges();

			respostaDto.Nome = novoUsuario.Nome;
			respostaDto.Username = novoUsuario.Username;
			respostaDto.Id = novoUsuario.Id;
		}
		catch (Exception excecao)
		{
			return StatusCode(400, excecao.Message);
		}

		return StatusCode(201, respostaDto);
	}
}




