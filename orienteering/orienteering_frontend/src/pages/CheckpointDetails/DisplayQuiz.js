import useAuthentication from "../../hooks/useAuthentication";
import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./AddQuizQuestion";
//

export default function DisplayQuiz() {
    return <p>dette er quiz</p>;
}