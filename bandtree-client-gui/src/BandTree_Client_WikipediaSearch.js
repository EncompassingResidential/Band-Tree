import React, { useState } from 'react';

function WikipediaSearch() {
  const [searchTerm, setSearchTerm]           = useState('');
  const [results, setResults]                 = useState('');
  const [isLoading, setIsLoading]             = useState(false);
  const [error, setError]                     = useState(null);
  const [displayedApiUrl, setDisplayedApiUrl] = useState('');

  const handleSearch = () => {
    if (!searchTerm) {
      alert('Please enter a Band, Group, or Artist search string.');
      return;
    }

    //              https://localhost:7088/wikipedia-search?searchTerm=aldo%20nova
    const encodedSearchTerm = encodeURIComponent(searchTerm);
    const apiUrl = `/wikipedia-search?searchTerm=${encodedSearchTerm}`;

    // Display the apiUrl before sending the request
    setDisplayedApiUrl(apiUrl);
    
    // Indicate that a request is in progress
    setIsLoading(true);

    fetch(apiUrl, {
      method: 'PUT', // Specify the HTTP method as PUT
      headers: {
        'Content-Type': 'application/json', // Set the content type if you are sending JSON data
        // Add any other headers as needed
      }
      // ,
      // body: JSON.stringify(sendBodyData), // Include the request payload if necessary
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Band Client calling Band Server HTTP error! Used URL ${apiUrl} Status: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        // Display the raw JSON response in the results text field
        setResults(JSON.stringify(data, null, 2));
        // Indicate that the request is complete
        setIsLoading(false);
        setError(null); // Clear any previous errors
      })
      .catch((error) => {
        console.error('Unknown Band Client to Band Server Error: ', error);
        // Set the error state to display a message to the user
        setError(
          <>
            Band Client: .catch error occurred while trying to fetch information from the Band Server.
            <br />
            with search term: {searchTerm}
            <br />
            and apiUrl: {apiUrl}
            <br />
            <br />
            {error.toString()}
            <br />
          </>
        );
        
        // Indicate that the request is complete
        setIsLoading(false);
      });
  };

  return (
    <div>
      <h1>Musician Family Tree Search via ! Wikipedia !</h1>
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
              Band Client Request URL: <code>{displayedApiUrl}</code>
            </p>
          )}
          {isLoading ? (
            <p>Loading...</p>
          ) : error ? (
            <p>{error}</p>
          ) : (
            <>
              <h2>Results</h2>
              <textarea
                rows="10"
                cols="50"
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
