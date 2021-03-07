import cv2
from tqdm import tqdm
import os


def cut(image, path):
    w, h = image.shape[1], image.shape[0]
    step_w, step_h = w // 5, h // 4
    for i in range(4):
        for j in range(5):
            crop_img = image[i * step_h: (i + 1) * step_h, j * step_w: (j + 1) * step_w]
            cv2.imwrite('./data/timelapse1_cut/' + str(i) + str(j) + '/' + path, crop_img)


if __name__ == "__main__":

    source_dir = './data/timelapse1_crop/'

    for file_name in tqdm(os.listdir(source_dir)):
        if file_name[0] == '.':
            continue
        img = cv2.imread(source_dir + file_name)
        cut(img, file_name)
