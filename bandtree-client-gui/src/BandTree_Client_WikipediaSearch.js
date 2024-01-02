import React, { useState } from 'react';

function WikipediaSearch() {
  const [searchTerm, setSearchTerm] = useState('');
  const [results, setResults] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSearch = () => {
    if (!searchTerm) {
      alert('Please enter a Band, Group, or Artist search string.');
      return;
    }

    //              https://localhost:7088
    const apiUrl = `https://localhost:7088/wikipedia-search?search=${searchTerm}`;

    
    // Indicate that a request is in progress
    setIsLoading(true);

    fetch(apiUrl)
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Band Client calling Band Server HTTP error! Status: ${response.status}`);
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
            Band Client: An error occurred while fetching data from Band Server.
            <br />
            {error.toString()}
          </>
        );
        
        // Indicate that the request is complete
        setIsLoading(false);
      });
  };

  return (
    <div>
      <h1>Wikipedia Search</h1>
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
