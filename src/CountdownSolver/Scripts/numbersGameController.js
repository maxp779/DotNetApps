import { Panel } from 'react-bootstrap';
import { NumbersGameView } from './numbersGameView.js';
import React from 'react';


class NumbersGameController extends React.Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleFocus = this.handleFocus.bind(this);
        this.resetForm = this.resetForm.bind(this);

        this.state = {
            number1:"",
            number2:"",
            number3:"",
            number4:"",
            number5:"",
            number6:"",
            target:"",
            currentAnswer:[],
            numbersMessage:<p>Solutions will appear here</p>,
            loading:false
        }
    }

    resetForm(event)
    {
        this.setState({
            number1:"",
            number2:"",
            number3:"",
            number4:"",
            number5:"",
            number6:"",
            target:"",
            currentAnswer:[],
            numbersMessage:<p>Solutions will appear here</p>,
            loading:false
        });
    }

    handleChange(event)
    {
        this.setState({[event.target.name]: event.target.value});
    }

    handleSubmit(event)
    {
        event.preventDefault();
        this.setState({currentAnswer:[], loading:true});
        let toServer =[this.state.number1,
            this.state.number2, 
            this.state.number3, 
            this.state.number4, 
            this.state.number5, 
            this.state.number6, 
            this.state.target];
        console.log("NUMBERS GAME");
        console.log(toServer);
        var request = new Request('/countdownsolver/countdownnumbers/' + JSON.stringify(toServer), {
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
                console.log(returnedObject);

                self.setState({currentAnswer:returnedObject});
                let numberOfSolutions = returnedObject.length;
                self.setState({loading:false, numbersMessage:<p>{numberOfSolutions} solutions found</p>});
            });
    }

    handleFocus(event)
    {
        event.target.select();
    }

    render() {
        return (
    <NumbersGameView 
    numbersMessage={this.state.numbersMessage}
    currentAnswer={this.state.currentAnswer}
    handleSubmit={this.handleSubmit}
    handleChange={this.handleChange}
    handleFocus={this.handleFocus}
    handleClick={this.resetForm}
    number1={this.state.number1}
    number2={this.state.number2}
    number3={this.state.number3}
    number4={this.state.number4}
    number5={this.state.number5}
    number6={this.state.number6}
    target={this.state.target}
    loading={this.state.loading}
    />
  );
    }
}

export {NumbersGameController}