import requests
import json

base_url_EndPoint  = "https://localhost:7088" 
url_EndPoint_bands = base_url_EndPoint + "/wikipedia-searchJOHN"

# JSON structure for the band
band_data = {
    "isbn": "56789012-34567",
    "title": "Python Testing Title 17",
    "author": "Python Tester",
    "shortDescription": "Python code - testing C# Server API - POSTing / Creating a band entry - https://localhost:7100/bands",
    "pageCount": 100,
    "releaseDate": "2012-03-04"
}


def post_band(url, band_data):
    headers = {'Content-Type': 'application/json'}
    
    try:
        response = requests.post(url, data=json.dumps(band_data), headers=headers, verify=False)
        response.raise_for_status()  # Raises an HTTPError if the HTTP request returned an unsuccessful status code
        
        return response.json()  # Returns the response JSON content

    except requests.exceptions.HTTPError as errh:
        print(f"Http Error: {errh}")
    except requests.exceptions.ConnectionError as errc:
        print(f"Error Connecting: {errc}")
    except requests.exceptions.Timeout as errt:
        print(f"Timeout Error: {errt}")
    except requests.exceptions.RequestException as err:
        print(f"Error: {err}")



def fetch_band(url):
    try:
        print (f"John url: \n{url}\n")
        response = requests.get(url, verify=False)
        response.raise_for_status()  # Raises an HTTPError if the HTTP request returned an unsuccessful status code

        # print (f"John response.text: \n{response.text}\n")
        
        # Assuming the API returns a JSON response containing an array of bands
        band = response.json()
        return band

    except requests.exceptions.HTTPError as errh:
        print(f"Http Error: {errh}")
        # raise # re-raise the exception
    except requests.exceptions.ConnectionError as errc:
        print(f"Error Connecting: {errc}")
        # raise # re-raise the exception
    except requests.exceptions.Timeout as errt:
        print(f"Timeout Error: {errt}")
        # raise # re-raise the exception
    except requests.exceptions.RequestException as err:
        print(f"Error: {err}")
        # raise # re-raise the exception


if __name__ == "__main__":
    # This code will only run if the script is executed directly,
    # not when it's imported as a module.

    # Posting the band - Create in CRUD
    response = post_band(url_EndPoint_bands, band_data)
    if response is not None:
        print(response)

    # Fetching the bands - Read in CRUD
    bands = fetch_band(url_EndPoint_bands)
    if bands is not None:
        for band in bands:
            print(band)  # Or process each band object as needed
