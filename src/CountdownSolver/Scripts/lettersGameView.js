import { Panel,Button,FormGroup,ControlLabel,FormControl,InputGroup,Row,Col } from 'react-bootstrap';
import React from 'react';

let listStyle = {
    "maxHeight": "300px",
    "overflowY":"scroll"
};

class LettersGameView extends React.Component {
    
    render() {
        return (
    <Panel header="Countdown letters helper" bsStyle="primary">
        <Row>
            <Col xs={12} md={6}>
                <form onSubmit={this.props.handleSubmit}>
                    <FormGroup>
                        <ControlLabel>Enter letters</ControlLabel>
                            <InputGroup>
                                <FormControl
                                    type="text"
                                    required="required"
                                    value={this.props.currentLetters}
                                    placeholder="e.g 'elomsftpg'"
                                    onChange={this.props.handleChange}
                                        />
                            <InputGroup.Button>
                                <Button bsStyle="primary" type="submit">Find words</Button>
                                <Button bsStyle="danger" type="button" onClick={this.props.handleClick}>Reset</Button>
                            </InputGroup.Button>
                        </InputGroup>
                    </FormGroup>
                </form>
            </Col>
        <Col xs={12} md={6} style={{"float":"right"}}>
            <Panel header="Words" bsStyle="info">
            {this.props.lettersMessage}
            <ul style={listStyle}>
            {this.props.wordsList.map(function(listValue){

                let decriptiveWord = "letters"
                if(listValue.length === 1)
                {
                    decriptiveWord = "letter";
                }

                return <li key={listValue}>{listValue} [{listValue.length} {decriptiveWord}]</li>;
            })}
            </ul>
        </Panel>
    </Col>
        </Row>
    </Panel>
      );
    }
}

export {LettersGameView}