
//function replaces polish symbols, just in case 
function replacePolishChars(text) {
    const polishToAscii = {
        ą: 'a',
        ć: 'c',
        ę: 'e',
        ł: 'l',
        ń: 'n',
        ó: 'o',
        ś: 's',
        ź: 'z',
        ż: 'z',
        Ą: 'A',
        Ć: 'C',
        Ę: 'E',
        Ł: 'L',
        Ń: 'N',
        Ó: 'O',
        Ś: 'S',
        Ź: 'Z',
        Ż: 'Z'
    }

    return text.replace(
        /[ąćęłńóśźżĄĆĘŁŃÓŚŹŻ]/g,
        (char) => polishToAscii[char] || char
    )
}

//apiKey from OpenWeatherMap API
const apiKey = '77c3a88dfaefeb225ff57827b1b21db9'
let cityInput = $('#SelectRestaurant');


function GetWeatherDetails(name, lat, lon, country, state)
{
    //forecast API
    let FORECAST_API_URL = `https://api.openweathermap.org/data/2.5/forecast?lat=${lat}&lon=${lon}&appid=${apiKey}&units=metric`;

    let dates = [];//needed for display porposes 
    let months = [];
    fetch(FORECAST_API_URL)
        .then((response) => {
            if (!response.ok) {
                throw new Error('City not found')
            }
            return response.json()
        })
        .then(data => {
            for (let i = 0; i < data.list.length; i++){

                //checks if day is already in list if so, skip
                let forecastDate = new Date(data.list[i].dt_txt);
                if (!dates.includes(forecastDate.getDate())) {
                    dates.push(forecastDate.getDate());
                    months.push(forecastDate.getMonth()+1);
                } 
            }

            for (let i = 0; i < dates.length; i++) {
                $('#weatherWidgetContainer').append(`
                <div class="orang">
                    <h4>${dates[i]}.${months[i]}</h4>
                    <p>Temp: ${data.list[i].main.temp}°C</p>
                    <img class="icon" src="https://openweathermap.org/img/wn/${data.list[i].weather[0].icon}@2x.png"">
                    </img>
                </div>`)
            }
        })
        .catch(() => {
            alert(`Fail to fetch weather`);
        });
}

function GetCityCoordinates() {
    let cityName = replacePolishChars($('#SelectRestaurant').val());

    //API for getting Coordinates needed for Forecast API
    let GEOCODING_API_URL = `https://api.openweathermap.org/geo/1.0/direct?q=${cityName}&limit=1&appid=${apiKey}`;

    fetch(GEOCODING_API_URL).then(res => res.json()).then(data => {
        let { name, lat, lon, country, state } = data[0];
        GetWeatherDetails(name, lat, lon, country, state);
    }).catch(() => {
        alert(`Failed to fetch cords ${cityName} `)
    })
}



//Hides icons if small window
$(window).resize(function () {
    if ($(window).width() < 960) {
        $('.icon').hide();
    }
});



$(document).ready(function () {
    //for small screens
    if ($(window).width() < 960) {
        $('.icon').hide();
    }

    //initailize weather View 
    GetCityCoordinates();

    //Select menu change function
    $('#SelectRestaurant').change(function () {
        $('#weatherWidgetContainer').html('');
        GetCityCoordinates();
    })
});

