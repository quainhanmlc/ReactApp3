import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../utils/api';
import './Login.css'; // Import CSS

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await api.post('/auth/login', { username, password });
            localStorage.setItem('token', response.data.token);

            const roles = response.data.roles;
            if (roles.includes('ADMIN')) {
                navigate('/teams');
            } else if (roles.includes('EMPLOYEE')) {
                navigate('/create-project');
            } else {
                alert('Unauthorized role');
            }
        } catch (error) {
            console.error('Login failed', error);
            alert('Login failed! Please check your credentials.');
        }
    };

    return (
        <div className="form-container">
            <h2>Login Here</h2>
            <form onSubmit={handleLogin}>
                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Login</button>
                <p>
                    <Link to="/forgot-password">Forgot Password?</Link>
                </p>
                <p>
                    <Link to="/register">Register</Link>
                </p>
            </form>
        </div>
    );
}

export default Login;
