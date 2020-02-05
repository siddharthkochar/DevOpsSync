import React from "react";
import map from "lodash/map";
import { Container, Card, CardImg, CardText, CardBody } from "reactstrap";

export default class ActionContainer extends React.Component {
  render() {
    const { services } = this.props;
    return (
      <Container style={{ marginTop: 30 }}>
        <div
          style={{
            fontSize: "70px",
            fontWeight: 700,
            margin: "50px"
          }}
        >
          {this.props.heading}
        </div>
        <div
          style={{
            flexWrap: "wrap",
            display: "flex",
            justifyContent: "center"
          }}
        >
          {map(services, service => (
            <Card
              inverse
              key={service.id}
              outline
              style={{
                backgroundColor: service.color,
                borderColor: service.color,
                textAlign: "left",
                width: "calc(30% - 40px)",
                margin: "20px",
                padding: 20,
                cursor: "pointer"
              }}
            >
              <CardBody>
                <CardText style={{ fontSize: "1.5rem", fontWeight: "bold" }}>
                  {service.name}
                </CardText>
                <CardText>{service.description}</CardText>
              </CardBody>
            </Card>
          ))}
        </div>
      </Container>
    );
  }
}
