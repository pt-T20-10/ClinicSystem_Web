import { useState } from 'react';
import { Routes, Route, Link, useNavigate } from 'react-router-dom';
import { Layout, Menu, Button, Avatar, Dropdown, message } from 'antd';
import { HomeOutlined, MedicineBoxOutlined, CalendarOutlined, FileTextOutlined, UserOutlined, LogoutOutlined } from '@ant-design/icons';

// --- CẬP NHẬT IMPORT CÁC TRANG THẬT TẠI ĐÂY ---
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import HomePage from './pages/HomePage';
import BookingPage from './pages/BookingPage'; // <--- MỚI: Import trang đặt lịch thật

// Các trang chưa làm thì giữ nguyên placeholder
const ProductsPage = () => <h1>Mua Thuốc (Sẽ làm sau)</h1>;
const BlogPage = () => <h1>Blog (Sẽ làm sau)</h1>;

const { Header, Content, Footer } = Layout;

const App = () => {

  const [user, setUser] = useState(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  });

  const navigate = useNavigate();
  
  const handleLogout = () => {
    localStorage.removeItem('user');
    setUser(null);
    message.success("Đã đăng xuất");
    navigate('/login');
  };
  
  // Menu Dropdown cho User
  const userMenu = {
    items: [
      { key: '1', label: 'Hồ sơ cá nhân', icon: <UserOutlined /> },
      { key: '2', label: 'Lịch sử khám', icon: <FileTextOutlined /> },
      { type: 'divider' },
      { key: '3', label: 'Đăng xuất', icon: <LogoutOutlined />, onClick: handleLogout, danger: true },
    ]
  };

  const items = [
    { key: '/', icon: <HomeOutlined />, label: <Link to="/">Trang chủ</Link> },
    { key: '/thuoc', icon: <MedicineBoxOutlined />, label: <Link to="/thuoc">Mua thuốc</Link> },
    { key: '/dat-lich', icon: <CalendarOutlined />, label: <Link to="/dat-lich">Đặt lịch</Link> },
    { key: '/blog', icon: <FileTextOutlined />, label: <Link to="/blog">Blog</Link> },
  ];

  return (
    <Layout className="layout" style={{ minHeight: '100vh' }}>
      <Header style={{ display: 'flex', alignItems: 'center', background: '#fff', borderBottom: '1px solid #f0f0f0', padding: '0 20px' }}>
        <div className="demo-logo" style={{ marginRight: '40px', fontWeight: 'bold', fontSize: '20px', color: '#03a9f4', display: 'flex', alignItems: 'center' }}>
           <MedicineBoxOutlined style={{ marginRight: 8, fontSize: 24, color: '#cddc39' }} /> 
           MEDICLINIC
        </div>
        <Menu theme="light" mode="horizontal" defaultSelectedKeys={['/']} items={items} style={{ flex: 1, borderBottom: 'none' }} />
        <div>
          {user ? (
            <Dropdown menu={userMenu} placement="bottomRight">
              <Button type="text" style={{ height: 64 }}>
                <Avatar style={{ backgroundColor: '#cddc39', verticalAlign: 'middle' }} size="large">
                  {user.fullName ? user.fullName.charAt(0).toUpperCase() : 'U'}
                </Avatar>
                <span style={{ marginLeft: 8, fontWeight: 500 }}>{user.fullName || user.phone}</span>
              </Button>
            </Dropdown>
          ) : (
            <div style={{ display: 'flex', gap: '10px' }}>
              <Link to="/login"><Button type="default">Đăng nhập</Button></Link>
              <Link to="/register"><Button type="primary" style={{ background: '#cddc39', borderColor: '#cddc39', color: '#000' }}>Đăng ký</Button></Link>
            </div>
          )}
        </div>
      </Header>
      
      <Content>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/thuoc" element={<ProductsPage />} />
          
          {/* Vì đã import BookingPage thật ở trên, Route này giờ sẽ render giao diện xịn */}
          <Route path="/dat-lich" element={<BookingPage />} />
          
          <Route path="/blog" element={<BlogPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Routes>
      </Content>
      
      <Footer style={{ textAlign: 'center', background: '#e1f5fe' }}>
        Clinic Management System ©2026 - <span style={{color: '#03a9f4'}}>Blue</span> & <span style={{color: '#afb42b'}}>Lime</span> Theme
      </Footer>
    </Layout>
  );
};

export default App;