using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/client")]
public class ClientController : ControllerBase
{
    public readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    // Get: api/v1/client => Read all client
    [HttpGet]
    public async Task<IActionResult> LoadClients()
    {
        List<ClientResDto> response = await _clientService.GetAllClients();
        return Ok(response);
    }

    // Get: api/v1/client/{clientId} => Read a client
    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetClient(Guid clientId)
    {
        ClientResDto? response = await _clientService.GetClientById(clientId);
        return response == null ? NotFound("Client not found!") : Ok(response);
    }

    // Get: api/v1/client/select => Select clients
    [HttpGet("select")]
    public async Task<IActionResult> SelectClients()
    {
        List<SelectClientRes> response = await _clientService.SelectClients();
        return Ok(response);
    }

    // Post: api/v1/client => Create a new client
    [HttpPost]
    public async Task<IActionResult> CreateClients(CreateClientDto clientData)
    {
        Guid newClientId = await _clientService.CreateClient(clientData);
        return Created($"/api/v1/client/{newClientId}", newClientId);
    }

    // Update: api/v1/client/{clientId} => Update a client
    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> UpdateClient(Guid clientId, [FromBody] CreateClientDto clientData)
    {
        bool result = await _clientService.UpdateClient(clientId, clientData);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "Client not found"
            });

        return Ok(new
        {
            success = true,
            message = "Client updated successfully"
        });
    }

    // Delete: api/v1/client/{clientId} => Delete a client
    [HttpDelete("{clientId:guid}")]
    public async Task<IActionResult> DeleteClient(Guid clientId)
    {
        bool response = await _clientService.DeleteClient(clientId);
        return response ? NoContent() : NotFound("Client not found!");
    }
}