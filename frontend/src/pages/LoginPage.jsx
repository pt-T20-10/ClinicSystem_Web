import React, { useState } from 'react';
import { Form, Input, Button, Checkbox, Card, Typography, message } from 'antd';
import { UserOutlined, LockOutlined, PhoneOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import axios from '../services/axios'; // Import file cấu hình axios

const { Title, Text } = Typography;

const LoginPage = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const onFinish = async (values) => {
    setLoading(true);
    try {
      // Gọi API đăng nhập Backend
      const response = await axios.post('/Auth/login', {
        phone: values.phone,
        password: values.password
      });

      // Nếu thành công
      message.success('Đăng nhập thành công!');
      
      // Lưu thông tin user vào localStorage để dùng sau này
      localStorage.setItem('user', JSON.stringify(response)); // response chứa userId, fullName...
      
      // Chuyển hướng về trang chủ
      navigate('/');
      
      // Reload lại trang để Header cập nhật trạng thái (cách đơn giản nhất)
      window.location.reload();

    } catch (error) {
      // Xử lý lỗi từ Backend trả về
      if (error.response && error.response.data) {
        message.error(error.response.data.message || 'Đăng nhập thất bại');
      } else {
        message.error('Lỗi kết nối server!');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      minHeight: '80vh', 
      background: '#e1f5fe' // Light Blue background
    }}>
      <Card style={{ width: 400, boxShadow: '0 4px 12px rgba(0,0,0,0.1)' }}>
        <div style={{ textAlign: 'center', marginBottom: 20 }}>
          <Title level={3} style={{ color: '#03a9f4', margin: 0 }}>Đăng Nhập</Title>
          <Text type="secondary">Chào mừng bạn quay trở lại phòng khám</Text>
        </div>

        <Form
          name="login"
          initialValues={{ remember: true }}
          onFinish={onFinish}
          layout="vertical"
          size="large"
        >
          <Form.Item
            name="phone"
            rules={[
              { required: true, message: 'Vui lòng nhập số điện thoại!' },
              { pattern: /^[0-9]{10}$/, message: 'Số điện thoại không hợp lệ!' }
            ]}
          >
            <Input prefix={<PhoneOutlined />} placeholder="Số điện thoại" />
          </Form.Item>

          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Mật khẩu" />
          </Form.Item>

          <Form.Item>
            <Form.Item name="remember" valuePropName="checked" noStyle>
              <Checkbox>Ghi nhớ đăng nhập</Checkbox>
            </Form.Item>
            <a style={{ float: 'right', color: '#03a9f4' }} href="">Quên mật khẩu?</a>
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block 
              style={{ background: '#03a9f4', borderColor: '#03a9f4' }}>
              Đăng nhập
            </Button>
          </Form.Item>

          <div style={{ textAlign: 'center' }}>
            Chưa có tài khoản? <Link to="/register" style={{ color: '#cddc39', fontWeight: 'bold' }}>Đăng ký ngay</Link>
          </div>
        </Form>
      </Card>
    </div>
  );
};

export default LoginPage;