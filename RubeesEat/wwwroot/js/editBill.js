function AddPerson() {
    const personSelect = document.getElementById("billPeople");
    const selectedOption = personSelect.options[personSelect.selectedIndex];
    const personId = selectedOption.value;
    const personName = selectedOption.text;

    if (document.getElementById("person-" + personId)) {
        alert("Diese Person ist bereits hinzugefügt.");
        return;
    }

    const addedPeopleDiv = document.getElementById("addedPeople");
    const personDiv = document.createElement("div");
    personDiv.classList.add("personDiv");
    personDiv.id = "person-" + personId;

    const personLabel = document.createElement("div");
    personLabel.classList.add("text");
    personLabel.innerText = personName;

    const amountInput = document.createElement("input");
    amountInput.type = "number";
    amountInput.step = ".01";
    amountInput.name = "amount" + personId;
    amountInput.required = true;
    amountInput.placeholder = "Betrag eingeben";

    const personIdInput = document.createElement("input");
    personIdInput.type = "hidden";
    personIdInput.name = "id" + personId;
    personIdInput.value = personId;

    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.innerText = "Remove";
    removeButton.onclick = function () {
        removePerson(personId);
    };

    personDiv.appendChild(personLabel);
    personDiv.appendChild(personIdInput);
    personDiv.appendChild(amountInput);
    personDiv.appendChild(removeButton);
    addedPeopleDiv.appendChild(personDiv);
}

function RemovePerson(personId) {
    const personDiv = document.getElementById("person-" + personId);
    if (personDiv) {
        personDiv.remove();
    }
}
