import { LettersGameController } from './lettersGameController.js';
import { NumbersGameController } from './numbersGameController.js';
import { CenterView } from './centerView.js';
import { Grid,Button,PageHeader,Row,Col } from 'react-bootstrap';
import React from 'react';
import ReactDOM from 'react-dom';

let headingStyle = 
{
    "display":"inherit",
    "margin":"0",
    "padding":"30px 0 10px",
    "textAlign":"center",
    "textShadow":"1px 1px 2px rgba(0,0,0,0.5)",
    "fontFamily":"\"Josefin Slab\",\"Helvetica Neue\",Helvetica,Arial,sans-serif",
    "fontSize":"3em","fontWeight":"700",
    "fontStyle": "italic",
    "lineHeight":"normal",
    "color":"#fff"
}

let subheadingStyle = 
{
    "display":"inherit",
    "margin":"0",
    "padding":"10px 0 10px",
    "textAlign":"center",
    "textShadow":"1px 1px 2px rgba(0,0,0,0.5)",
    "fontFamily":"\"Josefin Slab\",\"Helvetica Neue\",Helvetica,Arial,sans-serif",
    "fontSize":"1em","fontWeight":"700",
    "lineHeight":"normal",
    "color":"#fff"
}

class Main extends React.Component{

    componentWillMount(){
        document.body.background="img/countdown.jpg";
    }

    render()
    {
        return (
        <div>
            <Grid>    
                <Row className="show-grid">
                    <Col xs={12}>
                        <div style={headingStyle} className="title">Countdown Solver</div>
                        <div style={subheadingStyle} className="animated lightSpeedIn">Helping you cheat at countdown since 2016</div>
                    </Col>
                </Row>
                <Row className="show-grid">
                    <Col xs={12}>
                        <LettersGameController />
                    </Col>
                </Row>
                <Row className="show-grid">
                    <Col xs={12}>
                        <NumbersGameController />
                    </Col>
                </Row>
            </Grid>
        </div>
            );
          }
}

ReactDOM.render(<Main/>, document.getElementById("main"));