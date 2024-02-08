using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrimeriaAPICSharp.DTO;
using PrimeriaAPICSharp.Model;
using System.Net.Http;

namespace PrimeriaAPICSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImovelController : Controller
    {
        private static List<Imovel> _imoveis = new List<Imovel>();

        private readonly HttpClient _httpClient;

        public ImovelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult GetImoveis()
        {
            return Ok(_imoveis);
        }


        [HttpGet("{id}")]
        public IActionResult GetImovel(Guid id)
        {

            var item = _imoveis.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult PostImovel([FromBody] ImovelDTO imovel)
        {
            if (imovel.endereco == null)
                return BadRequest();

            Imovel imovelEntity = new Imovel(imovel.endereco, imovel.proprietario);

            _imoveis.Add(imovelEntity);

            return Ok(imovelEntity);
        }
        
        [HttpPost("{cep}/{proprietario}")]
        public async Task<IActionResult> PostImovelAsync(string cep, string proprietario)
        {
            string url = @"https://viacep.com.br/ws/" + $"{cep}"+ @"/json/";
            var response = await _httpClient.GetStringAsync(url);
            Endereco endereco = JsonConvert.DeserializeObject<Endereco>(response);


            if (endereco == null)
            {
                return NotFound();
            }

            Imovel imovel = new Imovel(endereco, proprietario);
            _imoveis.Add(imovel);

            return Ok(imovel);
        }

        [HttpPut]
        public IActionResult PutItem(Imovel item)
        {
            var itemIndex = _imoveis.FindIndex(i => i.Id == item.Id);

            if (itemIndex < 0)
            {
                return NotFound();
            }

            _imoveis[itemIndex] = item;

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var item = _imoveis.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            _imoveis.Remove(item);

            return NoContent();
        }
    }
}
