import React, { useState } from 'react';

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
        <table>
            <thead>
                <tr>
                <th>Page ID</th>
                <th>Title</th>
                <th>Snippet</th>
                <th>timestamp</th>
                </tr>
            </thead>
            <tbody>
                {searchWikiResults.search && searchWikiResults.search.length > 0 ? 
                    (  searchWikiResults.search.map((result, arrayindex) => (
                    <tr key={arrayindex}> {/* Ideally, use a unique identifier instead of index */}
                    <td>{result.pageid}</td>
                    <td>{result.title}</td>
                    <td dangerouslySetInnerHTML={{ __html: result.snippet }}></td>
                    <td>{result.timestamp}</td>
                </tr>
                    ))
                ) : ( {/* No results found - default rows */}
                    <>
                    <tr>
                    <td colSpan={4}>     Sigh.</td>
                    </tr>
                    <tr>
                        <td colSpan={4}>No Wikiepedia</td><td>Keep searching for that artist,</td>
                    </tr>
                    <tr>
						<td colSpan={4}> results found.</td><td>maybe with a different spelling...</td>
					</tr>
                    </>
                        )}
            </tbody>
        </table>
    );
}

export default SearchWikiResultsList;