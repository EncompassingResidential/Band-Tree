import React, { useState } from 'react';

function WikipediaSearch({ onResultsFetched }) {
  const [searchTerm, setSearchTerm]           = useState('');
  const [results, setResults]                 = useState('');
  const [isLoading, setIsLoading]             = useState(false);
  const [error, setError]                     = useState(null);
  const [displayedApiUrl, setDisplayedApiUrl] = useState('');
  const [showJSONResults, setShowResults]     = useState(false);

  const handleSearch = () => {
    if (!searchTerm) {
      alert('Please enter a Band, Group, or Artist search string.');
      return;
    }

    //              https://localhost:7088/wikipedia-search?searchTerm=aldo%20nova
    const encodedSearchTerm = encodeURIComponent(searchTerm);
    const apiUrl = `https://localhost:7088/wikipedia-search?searchTerm=${encodedSearchTerm}`;

    // Display the apiUrl before sending the request
    setDisplayedApiUrl(apiUrl);
    
    // Indicate that a request is in progress
    setIsLoading(true);

    fetch(apiUrl, {
      method: 'GET', // Specify the HTTP method as GET since no data on the server is being modified
      headers: {
        'Content-Type': 'application/json', // Set the content type if you are sending JSON data
        // Add any other headers as needed
      }
      // ,
      // body: JSON.stringify(sendBodyData), // Include the request payload if necessary
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Band Client HTTP error calling Band Server! Used URL ${apiUrl} Status: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        // Display the raw JSON response in the results text field
        setResults(JSON.stringify(data, null, 2));
        // After fetching data:
        onResultsFetched(data.query);
        // Indicate that the request is complete
        setIsLoading(false);
        setError(null); // Clear any previous errors
      })
      .catch((error) => {
        console.error('.catch Unknown Band Client to Band Server Error: ', error);
        // Set the error state to display a message to the user
        // If the error is "TypeError: NetworkError when attempting to fetch resource."
        // then the Band Server is not running is one possible reason.
        setError(
          <>
            Band Client: .catch error occurred while trying to fetch information from the Band Server.
            <br />
            with search term: {searchTerm}
            <br />
            and apiUrl: {apiUrl}
            <br />
            <br />
            ( {error.toString()} )
            <br />
          </>
        );
        
        // Indicate that the request is complete
        setIsLoading(false);
      });
  };

  const toggleResultsVisibility = () => {
    setShowResults(!showJSONResults); // Toggle between true and false
  };
  

  return (
    <div>
      <h1>Musician Family Tree Search via ! Wikipedia !</h1>
      <h2>version Wed, 01/03/2024 15:41</h2>
      <div>
        <input
          type="text"
          placeholder="Band, Group, or Artist search string."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button onClick={handleSearch}>Search if Artist or Band exists in English WikiPedia</button>
      </div>
      <div>
        {displayedApiUrl && (
            <p>
              Band Client URL Request sent to Band Server:
              <br />
               <code>{displayedApiUrl}</code>
            </p>
          )}
          <button onClick={toggleResultsVisibility}>
            {showJSONResults ? "Hide JSON Search Results" : "Show JSON Search Results"}
          </button>
          {isLoading ? (
            <p>Loading...</p>
          ) : error ? (
            <p>{error}</p>
          ) : showJSONResults && (
            <>
              <h2>Raw JSON Results via Band Server from Wikipedia</h2>
              <textarea
                rows="25"
                cols="150"
                value={results}
                readOnly
              />
            </>
          )}
      </div>
    </div>
  );
}

export default WikipediaSearch;
