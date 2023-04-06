const getResponse = (method, url) => {
  let xhr = new XMLHttpRequest();
  xhr.open(method, url);
  xhr.send(null);

  return new Promise((resolve, reject) => {
    xhr.addEventListener("load", () => {
      resolve(xhr.responseText);
    });
  });
};

const createTopMessage = (id_client, id_bouquet) => {
  let result = document.createElement("div");
  result.classList.add("top-messsage");
  result.innerHTML = `
    <input type="text" id="nbMonth" value="1">
    <button class="button" id="cancel">Fermer</button>
    <button class="button" id="confirm">Confirmer</button>
  `;

  document.body.appendChild(result);

  let cancel = document.getElementById("cancel");
  cancel.addEventListener("click", () => {
    document.body.removeChild(result);
  });

  let confirm = document.getElementById("confirm");
  confirm.addEventListener("click", async () => {
    let nbMonth = document.getElementById("nbMonth").value;
    let url = `/Client/renewal?id_client=${id_client}&id_bouquet=${id_bouquet}&nbMonth=${nbMonth}`;
    let method = "GET";

    let response = await getResponse(method, url);
    console.log(response);
  });
};

let id_client = document.getElementById("id_client").innerHTML;
let allBouquets = document.querySelectorAll(".bouquet");
allBouquets.forEach((bouquet) => {
  bouquet.addEventListener("click", () => {
    let id_bouquet = bouquet.getAttribute("data-id_bouquet");
    console.log(id_bouquet);
    createTopMessage(id_client, id_bouquet);
  });
});
