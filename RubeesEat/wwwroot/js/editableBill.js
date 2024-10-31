let counter = 0;
function addExistingEntries(entryLines)
{
    if (!entryLines)
        return;

    for (let i = 1; i < entryLines.length; i++)
    {
        const entryLine = entryLines[i];
        const dropdown = document.getElementById("billPeople");
        const addPerson = document.getElementById("addPerson");
        const selectElement = document.getElementById('billPeople');
        const amount = -entryLine.amount;
        const selectedPerson =  entryLine.person.firstName + " " + entryLine.person.lastName;
        const selectedId = entryLine.person.id;
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
        amountElement.value = amount;
        if (isTest)
            amountElement.setAttribute("data-test-id", "input");
        personDiv.append(amountElement);
        personDiv.append(document.createElement("br"));

        const button = document.createElement("button");
        button.innerText = "Entfernen";
        button.onclick = function () {
            personDiv.remove();
            let addToOptions = document.createElement("option");
            addToOptions.value = selectedPerson;
            addToOptions.dataset.id = selectedId;
            addToOptions.textContent = selectedPerson;
            selectElement.appendChild(addToOptions);

            addPerson.hidden = false;
        }
        if (isTest)
        {
            button.setAttribute("data-test-action", "removePeople");
            button.setAttribute("data-test-click-behavior", "Click");
        }
        personDiv.appendChild(button);
        document.getElementById("addedPeople").appendChild(personDiv);

        counter++;


    };
}

document.addEventListener('DOMContentLoaded', () => {
    const existingEntryLines = window["existingEntryLines"];
    console.log(existingEntryLines);
    addExistingEntries(existingEntryLines);  
});

function AddPersonByDropdown() {
    const selectElement = document.getElementById('billPeople');
    const selectedPerson = selectElement.value;
    const selectedId = selectElement.selectedOptions[0].dataset["id"];
    const personDiv = document.createElement('div');
    const removeButton = document.createElement("button");

    AddPerson(selectedId, selectedPerson, personDiv, removeButton);

    const addPerson = document.getElementById("addPerson");
    removeButton.onclick = function () {
        personDiv.remove();
        let addToOptions = document.createElement("option");
        addToOptions.value = selectedPerson;
        addToOptions.dataset.id = selectedId;
        addToOptions.textContent = selectedPerson;
        selectElement.appendChild(addToOptions);
        addPerson.hidden = false;
    }

    const dropdown = document.getElementById("billPeople");
    dropdown.remove(dropdown.selectedIndex)
    if (selectElement.options.length === 0) {
        addPerson.hidden = true;
    }
}

function AddPersonByButton(selectedId, selectedPerson) {
    const personDiv = document.createElement('div');
    const removeButton = document.createElement("button");

    AddPerson(selectedId, selectedPerson, personDiv, removeButton);

    removeButton.onclick = function () {
        personDiv.remove();
        document.getElementById('btn-' + selectedId).disabled = false;
    }
    document.getElementById('btn-' + selectedId).disabled = true;
}

function AddPerson(selectedId, selectedPerson, personDiv, removeButton){
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
    document.getElementById("addedPeople").appendChild(personDiv);

    removeButton.innerText = "Remove";
    personDiv.appendChild(removeButton);
    if (isTest)
    {
        removeButton.setAttribute("data-test-action", "removePeople");
        removeButton.setAttribute("data-test-click-behavior", "Click");
    }
    counter++;
}

function validate(formName) {
    const date = document.getElementById("billDate");

    const description = document.getElementById("billDescription");

    const amounts = document.getElementById("addedPeople").querySelectorAll("input[name^='amount']");

    for (let i = 0; i < amounts.length; i++) {
        if (!amounts[i].value) {
            document.getElementById("placeForErrorMessage").textContent = "Deine Eingabe ist ungültig. Bitte eine positive Zahl eingeben.";
            return;
        }
        if (amounts[i].value <= 0) {
            document.getElementById("placeForErrorMessage").textContent = amounts[i].value + " ist ungültig. Bitte eine positive Zahl eingeben.";
            return;
        }
        const amountSplitted = amounts[i].value.split(".");
        if (amountSplitted[1] && amountSplitted[1].length > 2) {
            document.getElementById("placeForErrorMessage").textContent = amounts[i].value + " ist ungültig. Bitte nur 2 Nachkommastellen eingeben.";
            return;
        }
    }

    const sum = [...amounts]
        .map((amount) => parseFloat(amount.value))
        .reduce((acc, value) => acc + value, 0);

    const totalAmount = parseFloat(document.getElementById("billAmount").value);

    if (!date.value) {
        document.getElementById("placeForErrorMessage").textContent = "Bitte ein Datum wählen.";
        return;
    }

    if (!description.value) {
        document.getElementById("placeForErrorMessage").textContent = "Bitte eine Beschreibung eingeben.";
        return;
    }

    if (counter === 0) {
        document.getElementById("placeForErrorMessage").textContent = "Bitte eine Person hinzufügen.";
        return;
    }

    if (isNaN(totalAmount)) {
        document.getElementById("placeForErrorMessage").textContent = "Bitte eine gültige Zahl für den Gesamtbetrag eingeben.";
        return;
    }

    if (totalAmount <= 0) {
        document.getElementById("placeForErrorMessage").textContent = totalAmount + " ist ungültig. Bitte eine gültige Zahl für den Gesamtbetrag eingeben.";
        return;
    }

    const totalAmountSplitted = totalAmount.toString().split(".");
    if (totalAmountSplitted[1] && totalAmountSplitted[1].length > 2) {
        document.getElementById("placeForErrorMessage").textContent = totalAmount + " ist ungültig. Bitte nur 2 Nachkommastellen eingeben.";
        return;
    }

    if (totalAmount !== sum) {
        document.getElementById("placeForErrorMessage").textContent = "Die Summe von den einzelnen Beträgen stimmt nicht mit dem Gesamtbetrag überein.";
        return;
    }

    document.getElementById(formName).submit();
}