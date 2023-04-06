using Microsoft.AspNetCore.Mvc;

public class ClientController: Controller {

  public IActionResult Clients() {
    List<string[]> clients = ClientModel.findAll();
    ViewData["list-clients"] = clients;
    return View();
  }

  [HttpGet]
  public IActionResult OneClient(string id_client) {
    string[] client = ClientModel.findOne(id_client);
    List<string[]> subscriptions = ClientModel.findAllSubscriptions(id_client);
    List<string[]> allBunches = ClientModel.findAllBunches(id_client);
    ViewData["client"] = client;
    ViewData["subscriptions"] = subscriptions;
    ViewData["all-bunches"] = allBunches;
    return View();
  }

  [HttpGet]
  public IActionResult renewal(string id_client, string id_bouquet, int nbMonth) {
    return Content(ClientModel.renewal(id_client, id_bouquet, nbMonth).ToString());
  }
}