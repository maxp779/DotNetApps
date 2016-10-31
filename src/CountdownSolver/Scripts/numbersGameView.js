import { Panel,Button,FormGroup,ControlLabel,FormControl,InputGroup,Row,Col } from 'react-bootstrap';
import { AboutPageModal } from './aboutPageModal';
import React from 'react';

let listStyle = {
    "maxHeight": "300px",
    "overflowY":"scroll"
};

let spacer = {
    "marginTop":"10px"
}

class NumbersGameView extends React.Component {
    
    render() {

        return (
    <Panel header="Countdown numbers helper" bsStyle="primary">
            <Row>
    <Col xs={12} sm={6} md={3}>
        <form onSubmit={this.props.handleSubmit}>
            <FormGroup>
                <ControlLabel>Enter numbers</ControlLabel>
                    <InputGroup>
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number1}
                            name="number1"
                            type="number"                         
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number2}
                            name="number2"
                            type="number"                         
                            required="required"
                        />
                        <FormControl
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number3}
                            name="number3"
                            type="number"                         
                            required="required"
                        />
                        <FormControl
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number4}
                            name="number4"
                            type="number"                         
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number5}
                            name="number5"
                            type="number"                         
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number6}
                            name="number6"
                            type="number"                         
                            required="required"
                        />
                    </InputGroup>
                    <ControlLabel style={spacer}>Enter target number</ControlLabel>
                    <InputGroup>
                       <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.target}
                            name="target"
                            type="number"                         
                            required="required"
                        />
                    <InputGroup.Button>
                        <Button bsStyle="primary" type="submit">Solve</Button>
                        <Button bsStyle="danger" type="button" onClick={this.props.handleClick}>Reset</Button>
                    </InputGroup.Button>
                    </InputGroup>
            </FormGroup>
        </form>
    </Col>
    <Col xs={12} md={6} style={{"float":"right"}}>
        <Panel header="Solutions" style={spacer} bsStyle="info">
            {this.props.numbersMessage}
            <ul style={listStyle}>
            {this.props.currentAnswer.map(function(listValue){
                return <li key={listValue}>{listValue}</li>;
            })}
            </ul>
        </Panel>
    </Col>
</Row>
<AboutPageModal/>
    </Panel>
      );
            }
}

export {NumbersGameView}