document.addEventListener('DOMContentLoaded', function () {
    const countrySelect = document.getElementById('country');
    const citySelect = document.getElementById('city');

    fetch('https://api.countrystatecity.in/v1/countries', {
        headers: {
            'X-CSCAPI-KEY': 'NHhvOEcyWk50N2Vna3VFTE00bFp3MjFKR0ZEOUhkZlg4RTk1MlJlaA=='
        }
    })
        .then(response => response.json())
        .then(data => {
            data.forEach(country => {
                const option = document.createElement('option');
                option.value = country.iso2;
                option.textContent = country.name;
                countrySelect.appendChild(option);
            });
        })
        .catch(error => console.error('خطأ في تحميل قائمة البلدان:', error));

    countrySelect.addEventListener('change', function () {
        const countryCode = this.value;
        citySelect.innerHTML = ''; // مسح الخيارات السابقة

        fetch(`https://api.countrystatecity.in/v1/countries/${countryCode}/cities`, {
            headers: {
                'X-CSCAPI-KEY': 'NHhvOEcyWk50N2Vna3VFTE00bFp3MjFKR0ZEOUhkZlg4RTk1MlJlaA=='
            }
        })
            .then(response => response.json())
            .then(data => {
                data.forEach(city => {
                    const option = document.createElement('option');
                    option.value = city.name;
                    option.textContent = city.name;
                    citySelect.appendChild(option);
                });
            })
            .catch(error => console.error('خطأ في تحميل قائمة المدن:', error));
    });
});