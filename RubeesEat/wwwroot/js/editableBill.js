let counter = 0;
function addExistingEntries(entryLines) {
    debugger;
    if (!entryLines)
        return;

    for (let i = 1; i < entryLines.length; i++) {
        const entryLine = entryLines[i];
        const selectElement = document.getElementById('billPeople');
        const amount = -entryLine.amount;
        const selectedPerson = entryLine.person.firstName + " " + entryLine.person.lastName;
        const selectedId = entryLine.person.id;

        const personDiv = document.createElement('div');
        personDiv.className = 'personDiv mb-3';

        const textDiv = document.createElement('div');
        textDiv.className = 'mb-1';
        textDiv.textContent = selectedPerson;
        personDiv.appendChild(textDiv);

        const personIdElement = document.createElement("input");
        personIdElement.type = "hidden";
        personIdElement.name = "id" + counter;
        personIdElement.value = selectedId;
        document.querySelector("#addedPeople").append(personIdElement);

        const amountGroup = document.createElement('div');
        amountGroup.className = 'd-flex align-items-center';

        const amountElement = document.createElement("input");
        amountElement.name = "amount" + counter;
        amountElement.type = "number";
        amountElement.step = ".01";
        amountElement.className = "form-control me-2";
        amountElement.value = amount;
        amountElement.oninput = updateTotalAmount;
        personDiv.appendChild(amountGroup);
        amountGroup.appendChild(amountElement);

        const button = document.createElement("button");
        button.innerText = "Entfernen";
        button.className = "btn btn-danger";
        button.onclick = function () {
            personDiv.remove();
            updateTotalAmount();
            let addToOptions = document.createElement("option");
            addToOptions.value = selectedPerson;
            addToOptions.dataset.id = selectedId;
            addToOptions.textContent = selectedPerson;
            selectElement.appendChild(addToOptions);

            document.getElementById("addPerson").hidden = false;
        };
        button.setAttribute("type", "button");
        amountGroup.appendChild(button);

        document.getElementById("addedPeople").appendChild(personDiv);
        counter++;
    }
}


document.addEventListener('DOMContentLoaded', () => {
    const existingEntryLines = window["existingEntryLines"];
    console.log(existingEntryLines);
    addExistingEntries(existingEntryLines);  
});

function updateTotalAmount() {
    const amounts = document.querySelectorAll("input[name^='amount']");
    let sum = 0;

    amounts.forEach((input) => {
        const value = Number(input.value) || 0;
        sum += value;
    });

    document.getElementById("billAmount").value = sum;
}


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
        updateTotalAmount();
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
    personDiv.className = 'personDiv mb-3';

    const textDiv = document.createElement('div');
    textDiv.className = 'mb-1';
    textDiv.textContent = selectedPerson + " ";
    personDiv.appendChild(textDiv);

    const personIdElement = document.createElement("input");
    personIdElement.type = "hidden";
    personIdElement.name = "id" + counter;
    personIdElement.value = selectedId;
    document.querySelector("#addedPeople").append(personIdElement);

    const amountGroup = document.createElement('div');
    amountGroup.className = 'd-flex algin-items-center';
    personDiv.appendChild(amountGroup);

    const amountElement = document.createElement("input");
    amountElement.name = "amount" + counter;
    amountElement.type = "number";
    amountElement.step = ".01";
    amountElement.className = "form-control me-2";
    if (isTest) {
        amountElement.setAttribute("data-test-id", "input");
    }
    amountGroup.appendChild(amountElement);

    amountElement.addEventListener('input', updateTotalAmount);

    removeButton.innerText = "Entfernen";
    removeButton.className = "btn btn-danger";
    removeButton.type = "button";
    amountGroup.appendChild(removeButton);

    document.getElementById("addedPeople").appendChild(personDiv);

    updateTotalAmount()

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

    const  addedPeople = document.getElementById("addedPeople");

    if (!addedPeople)
        return;

    const amounts = addedPeople.querySelectorAll("input[name^='amount']");

    for (let i = 0; i < amounts.length; i++) {
        if (!amounts[i].value) {
            document.getElementById("placeForErrorMessage").textContent = "Deine Eingabe ist ungültig. Bitte eine positive Zahl eingeben.";
            return;
        }

        const amount = Number(amounts[i].value);
        if (isNaN(amount) || amount <= 0) {
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
        .map((amount) => Number(amount.value))
        .reduce((acc, value) => acc + value, 0);

    const totalAmount = Number(document.getElementById("billAmount").value);

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

    if (isNaN(totalAmount) || totalAmount <= 0) {
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

$("#billAmount").focus(function(){
    $(this).blur();
});