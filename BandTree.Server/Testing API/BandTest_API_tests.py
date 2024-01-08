import pytest
import requests_mock
import urllib.parse
from BandTest_API_Calls import fetch_band, post_band  # Import your functions here
import sys
import os
import json

# https://localhost:7088/wikipedia-search?searchTerm=Microsoft
base_url_EndPoint  = "https://localhost:7088"
url_EndPoint_wikipedia_search = base_url_EndPoint + "/wikipedia-search"

# running this from PowerShell is
# get-date; pytest -s .\BandTest_API_tests.py; get-date
#
# BASH shell is
# date; pytest -s ./BandTest_API_tests.py; date

@pytest.fixture
def mock_request():
    with requests_mock.Mocker() as m:
        yield m

class BandFetchError(Exception):
    """Exception raised when there is an issue fetching band data."""
    pass

# create or update with BASH shell
# export USE_MOCK='True'
if os.getenv('USE_MOCK') == 'True':
    with requests_mock.Mocker() as m:
        # Set up your mock requests here
        m.get('http://example.com/mock', json={'mocked': True})

        # Your test code that uses the mock
else:
  # test code that makes real requests
   print(f"John USE_MOCK: {os.getenv('USE_MOCK')}")

# took out the mock_request fixture
def test_get_band():
    # Mocking the GET request
    # print(f"John sys.path: {sys.path}")
    query_params = {'searchTerm': 'rush band'}
    encoded_query_string = urllib.parse.urlencode(query_params)
    test_url = f"{url_EndPoint_wikipedia_search}?{encoded_query_string}"

    mock_base_data = base_get_rush_data_JSON  # Example response data

    # https://localhost:7088/wikipedia-search?searchTerm=band%20toto
    # https://localhost:7088/wikipedia-search?searchTerm=rush%20band
    # print (f"John mock_request.get(test_url): \n{test_url}\n")
    # mock_request.get(test_url)

    # Call the function, check for exception and assert
    try:
      print (f"John fetch_band with test_url: \n{test_url}\n")
      response_json = fetch_band(test_url)
      if response_json is None:
        # raise BandFetchError("No data returned from BandServer")
        response_json = "No data returned from BandServer"

    except BandFetchError as e:
      print(f"BandFetchError: {e}")
      assert False

    print(f"Base RUSH Data Expected Beginning 50: {mock_base_data[:100]}")
    print(f"BandServer response          1st  50: {str(response_json)[:100]}\n")
    print(f"Base RUSH Data Expected      LAST 50: {mock_base_data[-50:]}")
    print(f"BandServer response          last 50: {str(response_json)[-50:]}")
    mock_slice_base_data = mock_base_data[:100]
    # print("type(response_json)")
    # print(type(response_json))
    reponse_slice = str(response_json)[:100]
    # print(f"Base RUSH Data Expected  : {mock_base_data}")
    # print(f"BandServer response_json : {response_json}")
    
    # worked
    response_dict = response_json

    # did not work
    # expected_dict = json.loads(mock_base_data)

    remove_dictionary_key_recursive(response_dict, 'timestamp')
    # remove_dictionary_key_recursive(expected_dict, 'timestamp')

    # assert response_dict == expected_dict
    assert reponse_slice == mock_slice_base_data



def test_post_band(mock_request):
    # Mocking the POST request
    test_url = "https://localhost:7088/band"
    band_data = {
        "isbn": "5789012-3456789",
        "title": "Testing Title 18",
        "author": "Python Tester",
        "shortDescription": "Python code - testing C# Server API - POSTing / Creating a book entry - https://localhost:7100/band",
        "pageCount": 202,
        "releaseDate": "2012-03-05"
    }
    mock_response = {'success': True, 'message': 'Book added'}  # Example response data
    mock_request.post(test_url, json=mock_response)

    # Call the function and assert
    response = post_band(test_url, band_data)
    assert response == mock_response

# 2068 characters then values after that are not deterministic from call to Wikipedia.
# >       assert reponse_slice == mock_slice_base_data
# E       assert "{'batchcompl...roffset': 10," == '{"batchcompl...roffset": 10,'
# E             - {"batchcomplete": "", "continue": {"sroffset": 10,
# E             ?  ^             ^  ^^  ^        ^   ^        ^
# E             + {'batchcomplete': '', 'continue': {'sroffset': 10,
# E             ?  ^             ^  ^^  ^        ^   ^        ^ 
base_get_rush_data_JSON = '''{'batchcomplete': '', 'continue': {'sroffset': 10, 'continue': '-||'}, 'query': {'searchinfo': {'totalhits': 16785}, 'search': [{'ns': 0, 'title': 'Rush (band)', 'pageid': 25432, 'size': 161231, 'wordcount': 15949, 'snippet': '<span class="searchmatch">Rush</span> was a Canadian rock <span class="searchmatch">band</span> that primarily comprised Geddy Lee (bass guitar, keyboards, vocals), Alex Lifeson (guitar) and Neil Peart (drums, percussion', 'timestamp': '2024-01-05T23:30:33Z'}, {'ns': 0, 'title': 'Big Time Rush (group)', 'pageid': 28047774, 'size': 59131, 'wordcount': 4702, 'snippet': 'Big Time <span class="searchmatch">Rush</span> is an American pop music boy <span class="searchmatch">band</span> formed in 2009. The group is composed of Kendall Schmidt, James Maslow, Logan Henderson, and Carlos PenaVega', 'timestamp': '2023-12-21T20:08:51Z'}, {'ns': 0, 'title': 'Rush (Rush album)', 'pageid': 176951, 'size': 17961, 'wordcount': 1682, 'snippet': '<span class="searchmatch">Rush</span> is the debut studio album by Canadian rock <span class="searchmatch">band</span> <span class="searchmatch">Rush</span>. It was released on March 18, 1974, in Canada by Moon Records, the group\'s own label, before', 'timestamp': '2023-12-20T13:49:30Z'}, {'ns': 0, 'title': 'Moving Pictures (Rush album)', 'pageid': 384063, 'size': 35823, 'wordcount': 3550, 'snippet': 'rock <span class="searchmatch">band</span> <span class="searchmatch">Rush</span>, released on February 12, 1981 by Anthem Records. After touring to support their previous album, Permanent Waves (1980), the <span class="searchmatch">band</span> started', 'timestamp': '2023-12-28T14:31:10Z'}, {'ns': 0, 'title': 'Hemispheres (Rush album)', 'pageid': 167437, 'size': 31176, 'wordcount': 2729, 'snippet': 'studio album by Canadian rock <span class="searchmatch">band</span> <span class="searchmatch">Rush</span>, released in October 1978 by Anthem Records. After touring to support the <span class="searchmatch">band\'s</span> previous release, A Farewell to', 'timestamp': '2023-11-22T21:14:47Z'}, {'ns': 0, 'title': 'Rush!', 'pageid': 72505709, 'size': 40030, 'wordcount': 2978, 'snippet': '<span class="searchmatch">Rush</span>! is the third studio album by Italian rock <span class="searchmatch">band</span> M�neskin, released on 20 January 2023 through Epic Records. It was preceded by the singles &quot;Mammamia&quot;', 'timestamp': '2024-01-02T20:37:48Z'}, {'ns': 0, 'title': 'Rush', 'pageid': 99473, 'size': 6992, 'wordcount': 877, 'snippet': 'Look up <span class="searchmatch">RUSH</span>, <span class="searchmatch">Rush</span>, <span class="searchmatch">rush</span>, or rushes in Wiktionary, the free dictionary. <span class="searchmatch">Rush</span>(es) may refer to: <span class="searchmatch">Rush</span>, Colorado <span class="searchmatch">Rush</span>, Kentucky <span class="searchmatch">Rush</span>, New York <span class="searchmatch">Rush</span> City, Minnesota', 'timestamp': '2023-12-19T03:00:32Z'}, {'ns': 0, 'title': 'Grace Under Pressure (Rush album)', 'pageid': 464454, 'size': 21350, 'wordcount': 1896, 'snippet': '<span class="searchmatch">band</span> <span class="searchmatch">Rush</span>, released April 12, 1984, on Anthem Records. After touring for the <span class="searchmatch">band\'s</span> previous album, Signals (1982), came to an end in mid-1983, <span class="searchmatch">Rush</span> started', 'timestamp': '2023-12-17T09:19:43Z'}, {'ns': 0, 'title': 'Chronicles (Rush album)', 'pageid': 568537, 'size': 8914, 'wordcount': 301, 'snippet': 'is a double compilation album by Canadian rock <span class="searchmatch">band</span> <span class="searchmatch">Rush</span>, released in 1990. The collection was the <span class="searchmatch">band\'s</span> first album to be released in the 1990s, though', 'timestamp': '2023-12-11T03:07:14Z'}, {'ns': 0, 'title': 'Rush discography', 'pageid': 7546657, 'size': 77366, 'wordcount': 3015, 'snippet': '<span class="searchmatch">Rush</span> was a Canadian progressive rock <span class="searchmatch">band</span> originally formed in August 1968, in the Willowdale neighbourhood of Toronto, Ontario. For the overwhelming', 'timestamp': '2023-12-11T02:32:17Z'}]}}'''

def remove_dictionary_key_recursive(d, key):
    if isinstance(d, dict):
        d.pop(key, None)  # Remove the key if it exists
        for value in d.values():
            remove_dictionary_key_recursive(value, key)  # Recursive call
    elif isinstance(d, list):
        for item in d:
            remove_dictionary_key_recursive(item, key)  # Recursive call
