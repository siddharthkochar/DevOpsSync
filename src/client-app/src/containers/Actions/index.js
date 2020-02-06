import React from "react";
import { Switch, Route } from "react-router-dom";
import ActionContainer from "./ActionContainer";
import Message from "./Message";

const vstsDevOpsActions = [
  {
    id: 2,
    name: "Update Work Item",
    color: "#000000",
    description: "This trigger fires every time work item is updated"
  },
  {
    id: 1,
    name: "Create Build",
    color: "#000000",
    description: "This trigger fires every time a build is created"
  },
  {
    id: 3,
    name: "Add User to Team",
    color: "#000000",
    description: "This trigger fires every time user is added to team"
  },
  {
    id: 4,
    name: "Add Days off",
    color: "#000000",
    description: "This trigger fires every time days off are added"
  },
  {
    id: 5,
    name: "Team Capacity",
    color: "#000000",
    description: "This trigger fires every time team capacity is updated"
  }
];

const githubActions = [
  {
    id: 1,
    name: "Any push",
    color: "#000000",
    description:
      "This trigger fires every time a push is made to the repository"
  },
  {
    id: 2,
    name: "Pull request for a specific repository",
    color: "#000000",
    description:
      "This Trigger fires every time a new pull request is opened or closed"
  },
  {
    id: 3,
    name: "Any new issue",
    color: "#000000",
    description:
      "This Trigger fires every time any new issue is opened in a repository you own or collaborate on"
  },
  {
    id: 4,
    name: "Any new closed issue",
    color: "#000000",
    description:
      "This trigger fires every time any issue is closed in a repository you own or collaborate on"
  }
];

const slackActions = [
  {
    id: 1,
    name: "Direct message",
    color: "#000000",
    description: "This action posts direct message to slack user"
  },
  {
    id: 2,
    name: "Post to channel",
    color: "#000000",
    description: "This actions posts message to a public channel"
  }
];

export default class Actions extends React.Component {
  constructor() {
    super();
    this.state = {};
  }

  renderGithub(props) {
    const onClick = () => props.history.push("/");
    return (
      <ActionContainer
        services={githubActions}
        heading={"CHOOSE TRIGGER"}
        onClick={onClick}
      ></ActionContainer>
    );
  }

  renderVSTS(props) {
    const onClick = () => {
      props.history.push("message/connectionsaved");
      setTimeout(() => props.history.push("/"), 3000);
    };
    return (
      <ActionContainer
        services={vstsDevOpsActions}
        heading={"CHOOSE ACTION"}
        onClick={onClick}
      ></ActionContainer>
    );
  }

  renderSlack(props) {
    const onClick = () => {
      props.history.push("message/connectionsaved");
      setTimeout(() => props.history.push("/"), 3000);
    };
    return (
      <ActionContainer
        services={slackActions}
        heading={"CHOOSE ACTION"}
        onClick={onClick}
      ></ActionContainer>
    );
  }

  render() {
    return (
      <Switch>
        <Route
          exact
          path="/services/action/github"
          render={this.renderGithub}
        ></Route>
        <Route
          exact
          path="/services/action/vsts"
          render={this.renderVSTS}
        ></Route>
        <Route
          exact
          path="/services/action/slack"
          render={this.renderSlack}
        ></Route>
        <Route exact path="/services/action/message/connectionsaved">
          <Message text="Connection Saved!" />
        </Route>
      </Switch>
    );
  }
}
