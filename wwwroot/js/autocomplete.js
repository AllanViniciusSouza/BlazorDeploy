window.initGoogleAutocomplete = (inputId, dotNetHelper) => {
    const input = document.getElementById(inputId);
    console.log("Input encontrado?", input);

    if (!input) return;

    const autocomplete = new google.maps.places.Autocomplete(input, {
        componentRestrictions: { country: 'br' },
        types: ['address']
    });

    autocomplete.addListener('place_changed', () => {
        const place = autocomplete.getPlace();
        console.log("Endereço selecionado:", place.formatted_address);
        if (!place.formatted_address) return;

        // Passa o endereço selecionado para o Blazor
        dotNetHelper.invokeMethodAsync('OnPlaceSelected', place.formatted_address);
    });
};

window.initPlaceAutocompleteElement = function (elementId, dotNetHelper) {
    const element = document.getElementById(elementId);

    if (!element) {
        console.warn("Elemento PlaceAutocomplete não encontrado");
        return;
    }

    element.addEventListener('gmpx-placeautocomplete:place_changed', () => {
        const place = element.value;
        console.log("Endereço selecionado:", place);

        dotNetHelper.invokeMethodAsync('OnPlaceSelected', place);
    });
};
