//const React = require('react');
import React, { useState } from 'react';

function WikipediaSearch() {
  const [searchTerm, setSearchTerm] = useState('');
  const [results, setResults] = useState('');

  const handleSearch = () => {
    if (!searchTerm) {
      alert('Please enter a search term.');
      return;
    }

    //              https://localhost:7088
    const apiUrl = `https://localhost:7088/wikipedia-search?search=${searchTerm}`;

    fetch(apiUrl)
      .then((response) => {
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        // Display the raw JSON response in the results text field
        setResults(JSON.stringify(data, null, 2));
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  };

  return (
    <div>
      <h1>Wikipedia Search</h1>
      <div>
        <input
          type="text"
          placeholder="Enter a search term"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button onClick={handleSearch}>Search</button>
      </div>
      <div>
        <h2>Results</h2>
        <textarea
          rows="10"
          cols="50"
          value={results}
          readOnly
        />
      </div>
    </div>
  );
}

export default WikipediaSearch;
