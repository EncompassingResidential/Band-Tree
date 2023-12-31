import React, { useState } from 'react';
import Button from 'react-bootstrap/Button'; // https://react-bootstrap.github.io/components/buttons/
import Table from 'react-bootstrap/Table';  // version 10.2.5 on 1/06/2024

// Wikipedia returns a JSON object with the following structure:
// But we only send to this Component the "query" object.
// which is why you don't see query below in referencing the JSON structure.
{/* { "batchcomplete": "",
    "continue": {
        "sroffset": 10,
        "continue": "-||"
    },
    "query": {
        "searchinfo": {
            "totalhits": 105393,
            "suggestion": "run",
            "suggestionsnippet": "run"
        },
        "search": [
            {
                "ns": 0,
                "title": "Rush",
                "pageid": 99473,
                "size": 6992,
                "wordcount": 877,
                "snippet": "Look up <span class=\"searchmatch\">RUSH</span>, <span class=\"searchmatch\">Rush</span>, <span class=\"searchmatch\">rush</span>, or rushes in Wiktionary, the free dictionary. <span class=\"searchmatch\">Rush</span>(es) may refer to: <span class=\"searchmatch\">Rush</span>, Colorado <span class=\"searchmatch\">Rush</span>, Kentucky <span class=\"searchmatch\">Rush</span>, New York <span class=\"searchmatch\">Rush</span> City, Minnesota",
                "timestamp": "2023-12-19T03:00:32Z"
            },
            {
                etc...
*/}
function SearchWikiResultsList({ searchWikiResults }) {

    // Ensure searchWikiResults is an array
    if (!Array.isArray(searchWikiResults.search)) {
        return <p>Wikipedia or Band Server did not send a properly formatted JSON structure.</p>;
    }

    return (
        <table className="table table-hover">
            <thead>
                <tr>
                <th>Page ID</th>
                <th>Page Title: Band, Artist, Group name</th>
                <th>Snippet</th>
                <th>Timestamp</th>
                </tr>
            </thead>
            <tbody>
                {searchWikiResults.search && searchWikiResults.search.length > 0 ? 
                (searchWikiResults.search.map((result, arrayindex) => (
                    <tr key={arrayindex}>
                    <td className="threeDCell threeDCellExclude">{result.pageid}</td>
                    <td className="threeDCell" onClick={() => handlePutRequest(result.pageid)}>{result.title}</td>
                    <td className="threeDCell" onClick={() => handlePutRequest(result.pageid)} dangerouslySetInnerHTML={{ __html: result.snippet }}></td>
                    <td className="threeDCell threeDCellExclude">{result.timestamp}</td>
                    </tr>
                ))) : (
                    <>
                    <tr>
                    <td colSpan={4} className="threeDCell threeDCellExclude">     Sigh.</td>
                    </tr>
                    <tr>
                        <td colSpan={4} className="threeDCell threeDCellExclude">No Wikiepedia</td>
                        <td className="threeDCell threeDCellExclude">Keep searching for that artist,</td>
                    </tr>
                    <tr>
						<td colSpan={4} className="threeDCell threeDCellExclude"> results found.</td>
                        <td className="threeDCell threeDCellExclude">maybe with a different spelling...</td>
					</tr>
                    </>
                )
                }
            </tbody>
        </table>
    );
}

const handlePutRequest = async (pageid) => {
    const putAPIUrl = `https://localhost:7088/wikipedia-page/${pageid}`;

    const response = await fetch(putAPIUrl, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      }
    });
  
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
  
    // Do something with the response if needed
};

export default SearchWikiResultsList;