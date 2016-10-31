import { Panel,Button,FormGroup,ControlLabel,FormControl,InputGroup,Row,Col } from 'react-bootstrap';
import { AboutPageModal } from './aboutPageModal';
import React from 'react';

class NumbersGameView extends React.Component {
    
    render() {
        let loadingSpinner;
        if (this.props.loading) {
            loadingSpinner = <div className="loader"></div>;
        }
        else{
            loadingSpinner = "";
        }
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
                            max="10000"
                            min="1"
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number2}
                            name="number2"
                            type="number"
                            max="10000"
                            min="1"
                            required="required"
                        />
                        <FormControl
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number3}
                            name="number3"
                            type="number"                         
                            max="10000"  
                            min="1"
                            required="required"
                        />
                        <FormControl
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number4}
                            name="number4"
                            type="number"
                            max="10000"
                            min="1"
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number5}
                            name="number5"
                            type="number"
                            max="10000"
                            min="1"
                            required="required"
                        />
                        <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.number6}
                            name="number6"
                            type="number"
                            max="10000"
                            min="1"
                            required="required"
                        />
                    </InputGroup>
                    <ControlLabel className="spacer">Enter target number</ControlLabel>
                    <InputGroup>
                       <FormControl 
                            onChange={this.props.handleChange}
                            onFocus={this.props.handleFocus}
                            value={this.props.target}
                            name="target"
                            type="number"
                            max="10000"
                            min="1"
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
        <Panel header="Solutions" className="spacer" bsStyle="info">
        {this.props.numbersMessage}
            <Row>
                <Col xs={12}>
        {loadingSpinner}
                </Col>
            </Row>

            <ul className="scrollableList">
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