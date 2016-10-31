import { Panel } from 'react-bootstrap';
import { LettersGameView } from './lettersGameView.js';
import React from 'react';

class LettersGameController extends React.Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleReset = this.handleReset.bind(this);

        this.state = {
            wordsList:[],
            currentLetters:"",
            lettersMessage:<p>Words will appear here</p>,
            loading:false
        }
    }
    
    handleReset(event)
    {
        this.setState({
            wordsList:[],
            currentLetters:"",
            lettersMessage:<p>Words will appear here</p>,
            loading:false
        });
    }


    handleChange(event)
    {
        this.setState({currentLetters: event.target.value});
    }

    handleSubmit(event)
    {
        event.preventDefault();
        this.setState({wordsList:[],
            lettersMessage:<p>Words will appear here</p>,
            loading:true});
        var request = new Request('/countdownsolver/countdownletters/' + this.state.currentLetters, {
            method: 'get', 
            mode: 'cors', 
            redirect: 'follow',
            headers: new Headers({
                'Content-Type': 'json'
            })
        });
        var self=this;
        fetch(request)
            .then(function(response) { 
                // Convert to JSON
                return response.json();
            }).then(function(returnedObject) {
                // Yay, `j` is a JavaScript object
                //console.log(returnedObject);

                //sort the array by string length
                returnedObject.sort(function(a, b){
                    // ASC  -> a.length - b.length
                    // DESC -> b.length - a.length
                    return b.length - a.length;
                });
                self.setState({loading:false});
                self.updateWordsListAndMessage(returnedObject);
            });
    }

    updateWordsListAndMessage(wordsList){

        if(wordsList.length > 1000)
        {
            let minifiedWordsList = [];
            for(let index = 0; index < 1000; index++)
            {
                minifiedWordsList[index] = wordsList[index];
            }
            this.setState({wordsList:minifiedWordsList, lettersMessage:<div><p>{wordsList.length} words found</p>
            <p>Only the first 1000 are shown</p></div>});
        }
        else
        {
            this.setState({wordsList:wordsList, lettersMessage:<p>{wordsList.length} words found</p>});
        }
    }

            render() {
                return (
            <LettersGameView 
        loading={this.state.loading}
        currentLetters={this.state.currentLetters}
        lettersMessage={this.state.lettersMessage}
        wordsList={this.state.wordsList} 
        handleSubmit={this.handleSubmit}
        handleChange={this.handleChange}
        handleReset={this.handleReset}
            />
      );
    }
}

export {LettersGameController}