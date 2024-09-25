let counter = 0;

function AddPerson() {
    const dropdown = document.getElementById("billPeople");
    const addPerson = document.getElementById("addPerson");
    const selectElement = document.getElementById('billPeople');
    const selectedPerson = selectElement.value;
    const selectedId = selectElement.selectedOptions[0].dataset["id"];

    const personDiv = document.createElement('div');
    personDiv.className = 'personDiv';

    const textDiv = document.createElement('div');
    textDiv.className = 'text';
    textDiv.textContent = selectedPerson + " ";
    personDiv.appendChild(textDiv);

    const personIdElement = document.createElement("input");
    personIdElement.type = "hidden";
    personIdElement.name = "id" + counter;
    personIdElement.value = selectedId;
    document.querySelector("#addedPeople").append(personIdElement);

    const amountElement = document.createElement("input");
    amountElement.name = "amount" + counter;
    amountElement.type = "number";
    amountElement.step = ".01";
    if (isTest) 
        amountElement.setAttribute("data-test-id", "input");
    personDiv.append(amountElement);
    personDiv.append(document.createElement("br"));

    const button = document.createElement("button");
    button.innerText = "Remove";
    button.className = "removePeople";
    button.onclick = function () {
        personDiv.remove();
        let addToOptions = document.createElement("option");
        addToOptions.value = selectedPerson;
        addToOptions.dataset.id = selectedId;
        addToOptions.textContent = selectedPerson;
        selectElement.appendChild(addToOptions);

        addPerson.hidden = false;
    }
    personDiv.appendChild(button);
    document.getElementById("addedPeople").appendChild(personDiv);
    
    counter++;

    dropdown.remove(dropdown.selectedIndex)
    if (selectElement.options.length === 0) {
        addPerson.hidden = true;
    }
}

function validate() {
    const description = document.getElementById("billDescription");

    const amounts = document.getElementById("addedPeople").querySelectorAll("input[name^='amount']");

    for (let i = 0; i < amounts.length; i++) {
        if (!amounts[i].value) {
            document.getElementById("placeForErrorMessage").textContent = "Your input is not valid. Please enter a positive number";
            return;
        }
        if (amounts[i].value <= 0) {
            document.getElementById("placeForErrorMessage").textContent = amounts[i].value + " is not valid. Please enter a positive number";
            return;
        }
        const amountSplitted = amounts[i].value.split(".");
        if (amountSplitted[1] && amountSplitted[1].length > 2) {
            document.getElementById("placeForErrorMessage").textContent = amounts[i].value + " is not valid. Please only enter 2 digits after the comma";
            return;
        }
    }

    const sum = [...amounts]
        .map((amount) => parseFloat(amount.value))
        .reduce((acc, value) => acc + value, 0);

    const totalAmount = parseFloat(document.getElementById("billAmount").value);

    if (!description.value) {
        document.getElementById("placeForErrorMessage").textContent = "Please enter a description.";
        return;
    }

    if (counter === 0) {
        document.getElementById("placeForErrorMessage").textContent = "Please add a person.";
        return;
    }

    if (isNaN(totalAmount)) {
        document.getElementById("placeForErrorMessage").textContent = "Please enter a valid number for total amount.";
        return;
    }

    if (totalAmount <= 0) {
        document.getElementById("placeForErrorMessage").textContent = totalAmount + " is not valid. Please enter a valid number for total amount.";
        return;
    }

    const totalAmountSplitted = totalAmount.toString().split(".");
    if (totalAmountSplitted[1] && totalAmountSplitted[1].length > 2) {
        document.getElementById("placeForErrorMessage").textContent = totalAmount + " is not valid. Please only enter 2 digits after the comma.";
        return;
    }

    if (totalAmount !== sum) {
        document.getElementById("placeForErrorMessage").textContent = "The sum of individual amounts doesn't match the total bill amount.";
        return;
    }

    document.getElementById("splitBillForm").submit();
}
